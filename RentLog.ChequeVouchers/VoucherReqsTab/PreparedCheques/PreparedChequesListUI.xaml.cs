using CommonTools.Lib45.PrintTools;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.VoucherReqsTab.PreparedCheques
{
    public partial class PreparedChequesListUI : UserControl
    {
        public PreparedChequesListUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                cZone.btnPrint.Click += (c, d)
                    => tbl.dg.AskToPrint("Outstanding Checks",
                                null, $"Total: {VM.TotalSum:N2}");
            };
        }


        private PreparedChequesListVM VM => DataContext as PreparedChequesListVM;
    }
}
