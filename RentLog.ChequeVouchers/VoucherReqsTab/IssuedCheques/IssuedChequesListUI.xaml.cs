using CommonTools.Lib45.PrintTools;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.VoucherReqsTab.IssuedCheques
{
    public partial class IssuedChequesListUI : UserControl
    {
        public IssuedChequesListUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                cZone.btnPrint.Click += (c, d)
                    => tbl.dg.AskToPrint("Issued Checks",
                                null, $"Total: {VM.TotalSum:N2}");
            };
        }


        private IssuedChequesListVM VM => DataContext as IssuedChequesListVM;
    }
}
