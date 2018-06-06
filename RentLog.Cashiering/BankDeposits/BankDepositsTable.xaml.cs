using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.Cashiering.BankDeposits
{
    public partial class BankDepositsTable : UserControl
    {
        public BankDepositsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.ConfirmToDelete<BankDepositDTO>(_ => _.ToString());
                dg.EnableOpenCurrent<BankDepositDTO>();
                dg.ScrollToEndOnChange();
            };
        }
    }
}
