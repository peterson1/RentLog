using System.Threading.Tasks;
using System.Windows;

namespace RentLog.Cashiering
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.SectionTabs.ItemsReplaced += (c, d)
                    => tabs.SelectedIndex = 0;
            };
        }


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
