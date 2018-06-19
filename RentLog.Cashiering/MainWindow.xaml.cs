using System.Threading.Tasks;
using System.Windows;

namespace RentLog.Cashiering
{
    public partial class MainWindow : Window
    {
        private int _pickedIndex;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                tabs.SelectionChanged += (c, d) =>
                {
                    if (tabs.SelectedIndex != -1)
                        _pickedIndex = tabs.SelectedIndex;
                };

                VM.SectionTabs.ItemsReplaced += (c, d)
                    => tabs.SelectedIndex = _pickedIndex;
            };
        }


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
