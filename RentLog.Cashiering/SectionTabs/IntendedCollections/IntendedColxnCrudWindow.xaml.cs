using CommonTools.Lib45.UIExtensions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RentLog.Cashiering.SectionTabs.IntendedCollections
{
    public partial class IntendedColxnCrudWindow : Window
    {
        public IntendedColxnCrudWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                txtPRnum.SelectAll();
                await Task.Delay(100);
                txtPRnum.SelectAll();
                txtPRnum.KeyUp += TxtPRnum_KeyUp;
                SetHandlersForNullable(txtRent    );
                SetHandlersForNullable(txtRights  );
                SetHandlersForNullable(txtElectric);
                SetHandlersForNullable(txtWater   );
                SetHandlersForNullable(txtRemarks );
                btnSave.KeyUp += BtnSave_KeyUp;
            };
        }


        private void BtnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                btnSave.MoveFocus(FocusNavigationDirection.Previous);
        }


        private void TxtPRnum_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                case Key.Return:
                    if (VM.Draft.PRNumber != 0)
                        txtPRnum.MoveFocusToNext();
                    break;
                default:
                    break;
            }
        }


        private void SetHandlersForNullable(TextBox ctrl)
        {
            ctrl.MoveFocusToNextOnEnterKey();
            ctrl.MoveFocusOnArrowKeys(false, false, true, true);
            ctrl.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Add)
                {
                    e.Handled = true;
                    VM.SaveDraftCmd.ExecuteIfItCan();
                }
            };
        }


        private IntendedColxnCrudVM VM => DataContext as IntendedColxnCrudVM;
    }
}
