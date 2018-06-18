using System.Threading.Tasks;
using System.Windows;

namespace RentLog.Cashiering.SectionTabs.IntendedCollections
{
    public partial class IntendedColxnCrudWindow : Window
    {
        public IntendedColxnCrudWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                await Task.Delay(200);
                txtPRnum.SelectAll();
            };
        }
    }
}
