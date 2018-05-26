using CommonTools.Lib45.ApplicationTools;
using RentLog.DomainLib45;
using System.Windows;

namespace RentLog.Cashiering
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Initialize<AppArguments>(args =>
            {
                var date = args.Collections.UnclosedDate();
                new MainWindowVM(date, args).Show<MainWindow>();
            });
        }
    }
}
