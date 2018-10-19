using RentLog.ImportBYF.DailyTransactions;
using System;
using System.Linq;

namespace RentLog.ImportBYF.RntQueries
{
    public static class RntDailyTxnQuery
    {
        public static DailyTransactionCell QueryRNT(this DailyTransactionRow row) => new DailyTransactionCell
        {
            TotalCollections = GetTotalCollections(row),
            TotalDeposits    = GetTotalDeposits(row),
        };


        private static decimal GetTotalCollections(DailyTransactionRow row)
        {
            var db = row.MainWindow.AppArgs.Collections.For(row.Date);
            if (db == null) return 0;
            return db.IntendedColxns.Values.SelectMany(_ => _.GetAll())
                         .Sum(_ => _.Actuals.Total);
        }


        private static decimal GetTotalDeposits(DailyTransactionRow row)
        {
            var db = row.MainWindow.AppArgs.Collections.For(row.Date);
            if (db == null) return 0;
            return db.BankDeposits.GetAll().Sum(_ => _.Amount);
        }
    }
}
