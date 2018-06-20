using System;
using System.Windows.Controls;
using System.Windows.Input;
using CommonTools.Lib11.DateTimeTools;
using System.Windows.Media;

namespace RentLog.ChequeVouchers.DcdrTab.ReportSettings
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
            if (!Keyboard.IsKeyDown(Key.LeftShift)) return;
            //VM.PassbookRepo.RecomputeBalancesFrom(VM.DateRange.Start);

            foreach (var date in VM.DateRange.Start.EachDayUpTo(VM.DateRange.End))
            {
                foreach (var dep in VM.AppArgs.Collections.For(date).BankDeposits.GetAll())
                {
                    var repo = VM.AppArgs.Passbooks.GetRepo(dep.BankAccount.Id);
                    repo.InsertDepositedColxn(dep, date);
                }
            }
        }


        private void ApplyTheme(DatePicker pickr)
        {
            pickr.CalendarOpened += (s, e)
                => pickr.Foreground = Brushes.Black;

            pickr.CalendarClosed += (s, e)
                => pickr.Foreground = Brushes.White;
        }


        private DcdrTabVM VM => DataContext as DcdrTabVM;
    }
}
