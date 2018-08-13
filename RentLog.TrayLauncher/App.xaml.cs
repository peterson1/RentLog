using CommonTools.Lib45.ApplicationTools;
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
using System.Windows;

namespace RentLog.TrayLauncher
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.Initialize<LauncherArguments>(_
                => new MainWindowVM(_).Show<MainWindow>(true));
        }
    }
}
