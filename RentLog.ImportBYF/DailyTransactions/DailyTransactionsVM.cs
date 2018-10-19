using System;
using System.Threading.Tasks;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;

namespace RentLog.ImportBYF.DailyTransactions
{
    [AddINotifyPropertyChangedInterface]
    public class DailyTransactionsVM
    {
        public DailyTransactionsVM(MainWindowVM mainWindowVM)
        {
            MainWindow  = mainWindowVM;
            DateChooser = new DateChooserVM(this);
            RefreshCmd  = R2Command.Async(LoadPickedDates, _ => !IsBusy);
        }


        public MainWindowVM       MainWindow  { get; }
        public DateChooserVM      DateChooser { get; }
        public IR2Command         RefreshCmd  { get; }
        public TransactionRowsVM  DailyRows   { get; } = new TransactionRowsVM();
        public bool               IsBusy      { get; private set; }
        public string             BusyText    { get; private set; }


        private async Task LoadPickedDates()
        {
            IsBusy   = true;
            BusyText = "Loading chosen dates ...";
            await Task.Delay(1);
            DailyRows.Fill(DateChooser.StartDate, 
                           DateChooser.EndDate,
                           MainWindow);
            BusyText = "";
            IsBusy   = false;

            for (int i = 0; i < DailyRows.Count; i++)
            {
                await Task.Delay(i * 100);
                DailyRows[i].RefreshCmd.ExecuteIfItCan();
            }
        }
    }
}
