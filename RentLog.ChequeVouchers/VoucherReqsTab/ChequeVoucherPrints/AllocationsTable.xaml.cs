using System.Windows.Controls;

namespace RentLog.ChequeVouchers.VoucherReqsTab.ChequeVoucherPrints
{
    public partial class AllocationsTable : UserControl
    {
        public AllocationsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dgMain .SelectedCellsChanged += (c, d) => dgMain .SelectedIndex = -1;
                dgTotal.SelectedCellsChanged += (c, d) => dgTotal.SelectedIndex = -1;
            };
        }
    }
}
