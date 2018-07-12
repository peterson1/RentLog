using CommonTools.Lib45.UIExtensions;
using System.Windows;

namespace RentLog.Cashiering.BankDeposits
{
    public partial class BankDepositEditWindow : Window
    {
        public BankDepositEditWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                SetHandlers(cmbDesc);
                SetHandlers(cmbBankAccts);
                SetHandlers(txtAmount);
                SetHandlers(dteDeposit);
                SetHandlers(txtDepSlip);
            };
        }


        private void SetHandlers(FrameworkElement ctrl)
        {
            ctrl.MoveFocusToNextOnEnterKey();
            ctrl.MoveFocusOnArrowKeys();
        }
    }
}
