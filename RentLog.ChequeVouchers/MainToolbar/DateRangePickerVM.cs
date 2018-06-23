using PropertyChanged;
using System;

namespace RentLog.ChequeVouchers.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class DateRangePickerVM
    {
        public DateRangePickerVM()
        {
            End   = DateTime.Now;
            Start = End.AddDays(-100);
        }


        public bool       IsVisible  { get; set; }
        public DateTime   Start      { get; set; }
        public DateTime   End        { get; set; }
    }
}
