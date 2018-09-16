using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.Authorization;
using System.Threading.Tasks;
using System.Windows;

namespace RentLog.DomainLib45.SoaViewers.MainWindow
{
    public partial class SoaViewerWindow : Window
    {
        public SoaViewerWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.RefreshCompleted += async (c, d) =>
                {
                    await ScrollToEndIfEditing();
                };
            };
        }


        private SoaViewerVM VM => DataContext as SoaViewerVM;


        private async Task ScrollToEndIfEditing()
        {
            if (!VM.AppArgs.CanEditLedgerStartBalance(false)) return;
            await Task.Delay(1000);
            tbl.dg.ScrollToEnd();
            await Task.Delay(1000);
            tbl.dg.ScrollToEnd();
        }
    }
}
