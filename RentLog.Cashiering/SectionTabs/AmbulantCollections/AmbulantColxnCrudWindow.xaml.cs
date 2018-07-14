using CommonTools.Lib45.UIExtensions;
using System.Windows;
using System.Windows.Input;

namespace RentLog.Cashiering.SectionTabs.AmbulantCollections
{
    public partial class AmbulantColxnCrudWindow : Window
    {
        public AmbulantColxnCrudWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                SetHandlers(txtPRnum);
                SetHandlers(txtAmount);
                SetHandlers(txtRemarks);
                SetHandlers(txtReceivedFrom);
            };
        }


        private void SetHandlers(FrameworkElement ctrl)
        {
            ctrl.MoveFocusToNextOnEnterKey();
            ctrl.MoveFocusOnArrowKeys();
            ctrl.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Add)
                {
                    e.Handled = true;
                    VM.SaveDraftCmd.ExecuteIfItCan();
                }
            };
        }


        private AmbulantColxnCrudVM VM => DataContext as AmbulantColxnCrudVM;
    }
}
