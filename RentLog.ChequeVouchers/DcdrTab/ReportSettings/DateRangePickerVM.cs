using PropertyChanged;
using System;

namespace RentLog.ChequeVouchers.DcdrTab.ReportSettings
{
    [AddINotifyPropertyChangedInterface]
    public class DateRangePickerVM
    {
        public DateRangePickerVM()
        {
            End   = DateTime.Now;
            Start = End.AddDays(-100);
        }


        public DateTime   Start   { get; set; }
        public DateTime   End     { get; set; }
    }
}
