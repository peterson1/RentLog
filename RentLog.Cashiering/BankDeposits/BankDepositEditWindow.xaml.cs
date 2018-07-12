using CommonTools.Lib45.UIExtensions;
using System.Windows;
using System.Windows.Input;

namespace RentLog.Cashiering.BankDeposits
{
    public partial class BankDepositEditWindow : Window
    {
        public BankDepositEditWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                SetHandlers(cmbDesc, false);
                SetHandlers(cmbBankAccts, false);
                SetHandlers(txtAmount);
                SetHandlers(dteDeposit);
                SetHandlers(txtDepSlip);
            };
        }


        private void SetHandlers(FrameworkElement ctrl, bool handleArrowKeys = true)
        {
            ctrl.MoveFocusToNextOnEnterKey();

            if (handleArrowKeys) ctrl.MoveFocusOnArrowKeys();

            ctrl.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Add)
                {
                    e.Handled = true;
                    VM.SaveDraftCmd.ExecuteIfItCan();
                }
            };
        }


        private BankDepCrudVM VM => DataContext as BankDepCrudVM;
    }
}
