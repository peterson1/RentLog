using PropertyChanged;
using System;

namespace RentLog.ChequeVouchers.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class DateRangePickerVM
    {
        public DateRangePickerVM(MainWindowVM mainWindowVM)
        {
            End   = DateTime.Now;
            Start = End.AddDays(-100);
            Main  = mainWindowVM;
        }


        public MainWindowVM  Main       { get; }
        public bool          IsVisible  { get; set; }
        public DateTime      Start      { get; set; }
        public DateTime      End        { get; set; }


        public void OnStartChanged()
        {
            if (IsVisible) Main?.ClickRefresh();
        }


        public void OnEndChanged()
        {
            if (IsVisible) Main?.ClickRefresh();
        }
    }
}
