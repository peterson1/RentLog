using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib45.PrintTools.PrintPreviewer2
{
    public partial class PrintPreviewer2Window1 : Window
    {
        public PrintPreviewer2Window1()
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
