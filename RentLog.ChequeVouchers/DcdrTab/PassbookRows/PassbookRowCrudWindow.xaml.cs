using System.Threading.Tasks;
using System.Windows;

namespace RentLog.ChequeVouchers.DcdrTab.PassbookRows
{
    public partial class PassbookRowCrudWindow : Window
    {
        public PassbookRowCrudWindow()
        {
            InitializeComponent();
            Loaded += async (a, b) =>
            {
                await Task.Delay(200);
                ctrlFirst.SelectAll();
            };
        }
    }
}
