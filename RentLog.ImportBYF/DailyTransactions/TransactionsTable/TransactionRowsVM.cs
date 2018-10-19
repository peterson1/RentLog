using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Linq;

namespace RentLog.ImportBYF.DailyTransactions
{
    public class TransactionRowsVM : UIList<DailyTransactionRow>
    {
        internal void Fill(DateTime startDate, DateTime endDate, MainWindowVM mainWindowVM)
        {
            var rows = startDate.EachDayUpTo(endDate)
                    .OrderByDescending(_ => _)
                    .Select(_ => new DailyTransactionRow(_, mainWindowVM));

            UIThread.Run(() => this.SetItems(rows));
        }
    }
}
