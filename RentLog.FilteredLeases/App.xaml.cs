using CommonTools.Lib45.ApplicationTools;
using RentLog.DomainLib45;
using System.Windows;

namespace RentLog.FilteredLeases
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Initialize<AppArguments>(args =>
            {
                new MainWindowVM(args).Show<MainWindow>();
            });
        }
    }
}
