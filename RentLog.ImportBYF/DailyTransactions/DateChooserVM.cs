using CommonTools.Lib11.DateTimeTools;
using PropertyChanged;
using System;

namespace RentLog.ImportBYF.DailyTransactions
{
    [AddINotifyPropertyChangedInterface]
    public class DateChooserVM
    {
        public DateChooserVM(DailyTransactionsVM dailyTransactionsVM)
        {
            EndDate   = GetLastPostedDate(dailyTransactionsVM);
            StartDate = EndDate.MonthFirstDay();
        }


        public DateTime  StartDate  { get; set; }
        public DateTime  EndDate    { get; set; }


        private DateTime GetLastPostedDate(DailyTransactionsVM dailyTransactionsVM)
        {
            var dir = dailyTransactionsVM.MainWindow.CacheDir;
            return CacheReader2.getLastClosedDay(dir);
        }
    }
}
