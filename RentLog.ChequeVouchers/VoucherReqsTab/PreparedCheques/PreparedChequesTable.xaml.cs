using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.VoucherReqsTab.PreparedCheques
{
    public partial class PreparedChequesTable : UserControl
    {
        public PreparedChequesTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<ChequeVoucherDTO>();
                dg.ConfirmToDelete<ChequeVoucherDTO>(_ => _.ToString());
            };
        }
    }
}
