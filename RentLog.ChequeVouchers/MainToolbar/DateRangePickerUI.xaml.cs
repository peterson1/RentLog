using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RentLog.ChequeVouchers.MainToolbar
{
    public partial class DateRangePickerUI : UserControl
    {
        public DateRangePickerUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                ApplyTheme(dteStart);
                ApplyTheme(dteEnd);

                dteStart.MouseRightButtonUp += DteStart_MouseRightButtonUp;
            };
        }

        private void DteStart_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (!Keyboard.IsKeyDown(Key.LeftShift)) return;
            //await VM.RecomputeBalances();
        }


        private void ApplyTheme(DatePicker pickr)
        {
            pickr.CalendarOpened += (s, e)
                => pickr.Foreground = Brushes.Black;

            pickr.CalendarClosed += (s, e)
                => pickr.Foreground = Brushes.White;
        }


        //private DcdrTabVM VM => DataContext as DcdrTabVM;
    }
}
