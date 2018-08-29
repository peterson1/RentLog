using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.ApplicationTools;
using RentLog.DomainLib45;
using RentLog.DomainLib45.DailyStatusReporter;
using RentLog.DomainLib45.Reporters;
using RentLog.DomainLib45.WithOverduesReport;
using System.Windows;

namespace RentLog.ReportLauncher
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.Initialize<AppArguments>(args => LaunchReport(args));
        }


        private void LaunchReport(AppArguments args)
        {
            switch (args.Param1.ToUpper())
            {
                case "OVERDUES":
                    WithOverduesReport.Show(args);
                    break;
                case "COLXNSMRY":
                    ColxnSummaryExcelWriter.Launch(args);
                    break;
                case "DAILYSTATUS":
                    DailyStatusReportVM.Launch(args);
                    break;
                case "GLRECAP":
                    GLRecapExcelWriter.Launch(args);
                    break;
                default: throw Bad.Arg("Param1", args.Param1);
            }
        }
    }
}
