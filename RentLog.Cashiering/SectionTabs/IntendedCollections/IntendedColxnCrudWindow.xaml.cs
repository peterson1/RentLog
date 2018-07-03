using CommonTools.Lib45.UIExtensions;
using CommonTools.Lib11.StringTools;
using System.Threading.Tasks;
using System.Windows;
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
                //txtPRnum.LostFocus += TxtPRnum_LostFocus;
                txtPRnum.SelectAll();
                await Task.Delay(100);
                txtPRnum.SelectAll();
                txtPRnum.KeyUp += TxtPRnum_KeyUp;
                //txtPRnum.MoveFocusToNextOnEnterKey();
                txtRent.MoveFocusToNextOnEnterKey();
                txtRights.MoveFocusToNextOnEnterKey();
                txtElectric.MoveFocusToNextOnEnterKey();
                txtWater.MoveFocusToNextOnEnterKey();
            };
        }


        //private void TxtPRnum_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (txtPRnum.Text.IsBlank())
        //        txtPRnum.Focus();
        //}


        private void TxtPRnum_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Return:
                    if (VM.Draft.PRNumber != 0)
                        txtPRnum.MoveFocusToNext();
                    break;
                default:
                    break;
            }
        }

        private IntendedColxnCrudVM VM => DataContext as IntendedColxnCrudVM;
    }
}
