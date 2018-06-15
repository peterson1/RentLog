using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.VoucherReqsTab.IssuedCheques
{
    public partial class IssuedChequesTable : UserControl
    {
        public IssuedChequesTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<ChequeVoucherDTO>();
            };
        }
    }
}
