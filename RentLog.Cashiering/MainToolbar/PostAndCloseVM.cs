using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RentLog.Cashiering.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class PostAndCloseVM
    {
        private MainWindowVM _main;


        public PostAndCloseVM(MainWindowVM mainWindowVM)
        {
            _main           = mainWindowVM;
            PostAndCloseCmd = R2Command.Relay(DoPostAndClose, _ => IsBalanced, "Post & Close");
        }


        public decimal  TotalDeposits     { get; private set; }
        public decimal  TotalCollections  { get; private set; }
        public decimal  TotalDifference   { get; private set; }
        public bool     IsBalanced        { get; private set; }
        public bool     HasDeposits       { get; private set; }

        public IR2Command  PostAndCloseCmd  { get; }
        public IR2Command  RefreshCmd => _main.RefreshCmd;


        public void UpdateTotals()
        {
            TotalDeposits    = _main.BankDeposits.TotalSum;
            TotalCollections = _main.SectionTabs.Sum(_ => _.SectionTotal)
                             + _main.CashierColxns.TotalSum
                             + _main.OtherColxns.TotalSum;
            TotalDifference  = Math.Abs(TotalDeposits - TotalCollections);
            IsBalanced       = TotalDeposits == TotalCollections;
            HasDeposits      = TotalDeposits != 0;
        }


        private void DoPostAndClose()
            => Alert.Confirm("Please Confirm",
                "Are you sure you want to close this day and open the next?", async () =>
                {
                    _main.StartBeingBusy("Posting and Closing ...");

                    await Task.Delay(1000 * 5);

                    UIThread.Run(() => MessageBox.Show($"Successfully posted collections for {_main.Date:d-MMM-yyyy}{L.F}"
                             + $"The next market day [{_main.Date.AddDays(1):d-MMM-yyyy}] is now open for encoding.",
                            "   Operation Successful", MessageBoxButton.OK, MessageBoxImage.Information));

                    _main.CloseWindow();
                });
    }
}
