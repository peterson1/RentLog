using CommonTools.Lib45.PrintTools;
using System.Windows;

namespace RentLog.DomainLib45.WithOverduesReport
{
    public partial class WithOverduesReportWindow : Window
    {
        public WithOverduesReportWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.PrintClicked += (c, d) => PrintThenClose();
            };
        }

        private void PrintThenClose()
        {
            tbl.dg.AskToPrint($"{WithOverduesReport.TITLE} for “{VM.PickedSection}”", 
                              $"as of {VM.AsOfDate:MMMM d, yyyy}",
                            VM.TotalsSummary, VM.AppArgs.MarketState.BranchName);
            this.Close();
        }


        private WithOverduesReportVM VM => DataContext as WithOverduesReportVM;
    }
}
