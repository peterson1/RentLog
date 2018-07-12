using CommonTools.Lib45.UIExtensions;
using System.Windows;

namespace RentLog.Cashiering.CashierCollections
{
    public partial class CashierColxnCrudWindow : Window
    {
        public CashierColxnCrudWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                txtPRnum   .MoveFocusToNextOnEnterKey();
                txtAmount  .MoveFocusToNextOnEnterKey();
                cmbLease   .MoveFocusToNextOnEnterKey();
                cmbBillCode.MoveFocusToNextOnEnterKey();

                txtPRnum   .MoveFocusOnArrowKeys();
                txtAmount  .MoveFocusOnArrowKeys();
            };
        }
    }
}
