using CommonTools.Lib45.PrintTools;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests
{
    public partial class FundReqsListUI : UserControl
    {
        public FundReqsListUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                cZone.btnPrint.Click += (c, d)
                    => tbl.dg.AskToPrint("For Check Preparation", 
                                null, $"Total: {VM.TotalSum:N2}");
            };
        }


        private FundReqsListVM VM => DataContext as FundReqsListVM;
    }
}
