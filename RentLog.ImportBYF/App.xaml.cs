using CommonTools.Lib45.ApplicationTools;
using RentLog.DomainLib45;
using RentLog.ImportBYF.Version2UI;
using System.Windows;

namespace RentLog.ImportBYF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Initialize<AppArguments>(args =>
            {
                //new MainWindowVM(args).Show<MainWindow>();
                new MainWindowVM2(args).Show<MainWindow2>();
            });
        }
    }
}
