using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RentLog.ImportBYF.Version2UI.LeaseBalancesPane.LeasesList
{
    [AddINotifyPropertyChangedInterface]
    public class LeasesListVM : UIList<LeaseRowVM>
    {
        public LeasesListVM(MainWindowVM2 mainWindowVM2)
        {
            MainWindow = mainWindowVM2;
        }


        public MainWindowVM2 MainWindow { get; }


        public void FillLeasesList()
        {
            var mkt     = MainWindow.AppArgs.MarketState;
            var actives = mkt.ActiveLeases.GetAll()
                             .OrderByDescending(_ => _.Id);
            var inactvs = mkt.InactiveLeases.GetAll()
                             .OrderByDescending(_ => _.Id);
            var both    = actives.Concat(inactvs)
                                 .Select(_ => new LeaseRowVM(_, MainWindow));
            UIThread.Run(() => SetItems(both));
        }


        public async Task RefreshAll()
        {
            foreach (var row in this)
            {
                if (!MainWindow.LeaseBalances.IsRunning) return;
                await row.RefreshCmd.RunAsync();
                CommandManager.InvalidateRequerySuggested();
                if (!MainWindow.LeaseBalances.IsRunning) return;

                if (row.IsValidImport) continue;

                if (row.UpdateRntCmd.CanExecute(null))
                    await row.UpdateRntCmd.RunAsync();

                await Task.Delay(100);
            }
        }
    }
}
