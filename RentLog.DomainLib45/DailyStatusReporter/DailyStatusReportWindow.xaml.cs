using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RentLog.DomainLib45.DailyStatusReporter
{
    public partial class DailyStatusReportWindow : Window
    {
        public DailyStatusReportWindow()
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
