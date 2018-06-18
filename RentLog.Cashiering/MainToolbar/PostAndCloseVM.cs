using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RentLog.Cashiering.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class PostAndCloseVM
    {
        public PostAndCloseVM(MainWindowVM mainWindowVM)
        {
            Main           = mainWindowVM;
            PostAndCloseCmd = R2Command.Relay(ConfirmExecution, _ => CanPostAndClose(), "Post & Close");
        }


        public MainWindowVM  Main              { get; }
        public decimal       TotalDeposits     { get; private set; }
        public decimal       TotalCollections  { get; private set; }
        public decimal       TotalDifference   { get; private set; }
        public bool          IsBalanced        { get; private set; }
        public bool          HasDeposits       { get; private set; }

        public IR2Command  PostAndCloseCmd  { get; }
        public IR2Command  RefreshCmd => Main.RefreshCmd;


        private bool CanPostAndClose()
        {
            if (!Main.CanReview) return false;
            if (Main.IsBusy) return false;
            return IsBalanced;
        }


        public void UpdateTotals()
        {
            TotalDeposits    = Main.BankDeposits.TotalSum;
            TotalCollections = Main.SectionTabs.Sum(_ => _.SectionTotal)
                             + Main.CashierColxns.TotalSum
                             + Main.OtherColxns.TotalSum;
            TotalDifference  = Math.Abs(TotalDeposits - TotalCollections);
            IsBalanced       = TotalDeposits == TotalCollections;
            HasDeposits      = TotalDeposits != 0;
        }


        private void ConfirmExecution()
            => Alert.Confirm("Are you sure you want to close this day and open the next?", 
                async () => await RunPostAndClose());


        private async Task RunPostAndClose()
        {
            Main.StartBeingBusy("Posting and Closing ...");

            await Task.Run(() =>
            {
                var jobs = MarketDayCloser.GetActions(Main.ColxnsDB, Main.AppArgs);
                Parallel.Invoke(jobs.ToArray());
            });

            MessageBox.Show($"Successfully posted collections for {Main.Date:d-MMM-yyyy}{L.F}"
                        + $"The next market day [{Main.Date.AddDays(1):d-MMM-yyyy}] is now open for encoding.",
                    "   Operation Successful", MessageBoxButton.OK, MessageBoxImage.Information);

            Main.CloseWindow();
        }
    }
}
