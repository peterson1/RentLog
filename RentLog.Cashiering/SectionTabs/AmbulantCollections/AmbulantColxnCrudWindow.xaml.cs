using CommonTools.Lib45.UIExtensions;
using System.Windows;

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
                SetHandlers(txtReceivedFrom);
            };
        }


        private void SetHandlers(FrameworkElement ctrl)
        {
            ctrl.MoveFocusToNextOnEnterKey();
            ctrl.MoveFocusOnArrowKeys();
        }
    }
}
