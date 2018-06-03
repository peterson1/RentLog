using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.ApplicationTools;
using RentLog.DomainLib45;
using RentLog.DomainLib45.Reporters;
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
                case "COLXNSMRY":
                    ColxnSummaryExcelWriter.Launch(args);
                    break;
                default: throw Bad.Arg("Param1", args.Param1);
            }
        }
    }
}
