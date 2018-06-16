using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests
{
    public partial class FundReqsTable : UserControl
    {
        public FundReqsTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<FundRequestDTO>();
                dg.ConfirmToDelete<FundRequestDTO>(_ => _.ToString());
            };
        }
    }
}
