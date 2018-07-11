using PropertyChanged;
using System;

namespace RentLog.ChequeVouchers.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class DateRangePickerVM
    {
        public DateRangePickerVM(MainWindowVM mainWindowVM)
        {
            Main  = mainWindowVM;
            End   = DateTime.Now;
            Start = End.AddDays(-100);
        }


        public MainWindowVM  Main       { get; }
        public bool          IsVisible  { get; set; }
        public DateTime      Start      { get; set; }
        public DateTime      End        { get; set; }
    }
}
