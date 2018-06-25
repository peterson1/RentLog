using System.Threading.Tasks;
using System.Windows;

namespace RentLog.DomainLib45.SoaViewers.MainWindow
{
    public partial class SoaViewerWindow : Window
    {
        public SoaViewerWindow()
        {
            InitializeComponent();
            //Loaded += async (a, b) =>
            //{
            //    await Task.Delay(500);
            //    VM.ClickRefresh();
            //};
        }


        //private SoaViewerVM VM => DataContext as SoaViewerVM;
    }
}
