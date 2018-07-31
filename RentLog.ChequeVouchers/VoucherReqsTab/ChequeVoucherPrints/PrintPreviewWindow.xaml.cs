using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RentLog.ChequeVouchers.VoucherReqsTab.ChequeVoucherPrints
{
    public partial class PrintPreviewWindow : Window
    {
        public PrintPreviewWindow()
        {
            InitializeComponent();
        }


        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
