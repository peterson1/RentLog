using CommonTools.Lib45.ApplicationTools;
using RentLog.DomainLib45;
using System.Windows;

namespace RentLog.StallsCrud
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Initialize<AppArguments>(args =>
            {
                new MainWindowVM(args).Show<MainWindow>();

                //var now = DateTime.Now.Date;
                //var bgn = new DateTime(now.Year, now.Month - 1, 1);
                //var end = bgn.MonthLastDay();
                //
                //if (!PopUpInput.TryGetDateRange("Dates covered by Collection Summary Report", out (DateTime Start, DateTime End) rng, bgn, end)) return;
                //new ColxnSummaryReport(rng.Start, rng.End, args).ToExcel();
            });
        }
    }
}
