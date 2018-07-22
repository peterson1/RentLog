using System.Windows;
using System.Windows.Controls;

namespace RentLog.Cashiering.MainToolbar
{
    public partial class MainToolbarUI : UserControl
    {
        public MainToolbarUI()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VM.IsCashierSubmitting = true;
        }


        private PostAndCloseVM VM => DataContext as PostAndCloseVM;
    }
}
