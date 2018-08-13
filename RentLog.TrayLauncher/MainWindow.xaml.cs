using System.Drawing;
using System.Windows;
using static RentLog.TrayLauncher.Properties.Settings;

namespace RentLog.TrayLauncher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                trayIcon.Icon = new Icon(Default.TrayIconFile);
            };
        }
    }
}
