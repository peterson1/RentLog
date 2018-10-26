using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList
{
    [AddINotifyPropertyChangedInterface]
    public class PeriodsListVM : UIList<PeriodRowVM>
    {
        public PeriodsListVM(MainWindowVM2 mainWindowVM2)
        {
            MainWindow = mainWindowVM2;
        }


        public MainWindowVM2 MainWindow { get; }


        public void FillPeriodsList((DateTime Min, DateTime Max) periods)
        {
            var list = periods.Min.EachDayUpTo(periods.Max)
                              .OrderByDescending(_ => _)
                              .Select(_ => new PeriodRowVM(_, MainWindow));
            UIThread.Run(() => SetItems(list));
        }


        public async Task RefreshAll()
        {
            foreach (var row in this)
            {
                if (!MainWindow.TransactionData.IsRunning) return;
                await row.RefreshCmd.RunAsync();
                CommandManager.InvalidateRequerySuggested();
                if (!MainWindow.TransactionData.IsRunning) return;

                if (row.IsValidImport) continue;

                if (row.UpdateRntCmd.CanExecute(null))
                    await row.UpdateRntCmd.RunAsync();

                await Task.Delay(100);
            }
        }
    }
}
