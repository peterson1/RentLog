using CommonTools.Lib45.UIExtensions;
using System.Windows;
using System.Windows.Input;

namespace RentLog.Cashiering.CashierCollections
{
    public partial class CashierColxnCrudWindow : Window
    {
        public CashierColxnCrudWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                SetHandlers(txtPRnum   );
                SetHandlers(txtAmount  );
                SetHandlers(cmbLease   , false);
                SetHandlers(cmbBillCode, false);
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


        private CashierColxnCrudVM VM => DataContext as CashierColxnCrudVM;
    }
}
