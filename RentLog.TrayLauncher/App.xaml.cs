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

            var tbi = new TaskbarIcon();
            tbi.Icon = new Icon("Icons/strawberry_29.ico");
            //tbi.IconSource = new BitmapImage(new Uri("Icons/strawberry_29.ico", UriKind.Relative));
        }
    }
}
