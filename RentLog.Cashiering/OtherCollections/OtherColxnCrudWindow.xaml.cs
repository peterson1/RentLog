using CommonTools.Lib45.UIExtensions;
using System.Windows;

namespace RentLog.Cashiering.OtherCollections
{
    public partial class OtherColxnCrudWindow : Window
    {
        public OtherColxnCrudWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                txtPRnum    .MoveFocusToNextOnEnterKey();
                txtAmount   .MoveFocusToNextOnEnterKey();
                cmbCollector.MoveFocusToNextOnEnterKey();
                cmbGLAcct   .MoveFocusToNextOnEnterKey();

                txtPRnum    .MoveFocusOnArrowKeys();
                txtAmount   .MoveFocusOnArrowKeys();
            };
        }
    }
}
