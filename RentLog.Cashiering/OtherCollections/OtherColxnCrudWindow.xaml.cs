using CommonTools.Lib45.UIExtensions;
using System.Windows;
using System.Windows.Input;

namespace RentLog.Cashiering.OtherCollections
{
    public partial class OtherColxnCrudWindow : Window
    {
        public OtherColxnCrudWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                SetHandlers(txtPRnum  );
                SetHandlers(txtAmount );
                SetHandlers(txtRemarks);
                SetHandlers(cmbCollector, false);
                SetHandlers(cmbGLAcct   , false);
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


        private OtherColxnCrudVM VM => DataContext as OtherColxnCrudVM;
    }
}
