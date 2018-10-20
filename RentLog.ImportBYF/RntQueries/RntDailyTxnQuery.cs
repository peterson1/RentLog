using RentLog.DomainLib11.CollectionRepos;
using RentLog.ImportBYF.DailyTransactions;
using System.Linq;

namespace RentLog.ImportBYF.RntQueries
{
    public static class RntDailyTxnQuery
    {
        public static DailyTransactionCell QueryRNT(this DailyTransactionRow row)
        {
            var db = row.MainWindow.AppArgs.Collections.For(row.Date);
            if (db == null) return new DailyTransactionCell();

            //var intendeds = GetIntendedColxnsTotal(db);
            //var others    = GetOtherColxnsTotal(db);
            //var cashiers  = GetCashierColxnsTotal(db);

            return new DailyTransactionCell
            {
                TotalCollections = GetIntendedColxnsTotal(db)
                                 + GetAmbulantColxnsTotal(db)
                                 + GetOtherColxnsTotal   (db)
                                 + GetCashierColxnsTotal (db),
                //TotalCollections = intendeds + others + cashiers,
                TotalDeposits    = GetTotalDeposits(db),
            };
        }


        private static decimal GetIntendedColxnsTotal(ICollectionsDB db)
        {
            //return db.IntendedColxns.Values.SelectMany(_
            //                => _.GetAll()).Sum(_ => _.Actuals.Total);

            //var labeld = db.IntendedColxns.Select(s
            //    => (s.Key, s.Value.GetAll().Sum(_ => _.Actuals.Total)));

            var bySec  = db.IntendedColxns.Values.Select(s 
                => s.GetAll().Sum(_ => _.Actuals.Total));

            var sum = bySec.Sum(_ => _);
            return sum;
        }


        private static decimal GetAmbulantColxnsTotal(ICollectionsDB db)
        {
            var bySec = db.AmbulantColxns.Values.Select(s
               => s.GetAll().Sum(_ => _.Amount));

            var sum = bySec.Sum(_ => _);
            return sum;
        }


        private static decimal GetOtherColxnsTotal(ICollectionsDB db)
        {
            //return db.OtherColxns.GetAll().Sum(_ => _.Amount);
            var all = db.OtherColxns.GetAll();
            var sum = all.Sum(_ => _.Amount);
            return sum;
        }

        private static decimal GetCashierColxnsTotal(ICollectionsDB db)
        {
            //return db.CashierColxns.GetAll().Sum(_ => _.Amount);
            var all = db.CashierColxns.GetAll();
            var sum = all.Sum(_ => _.Amount);
            return sum;
        }

        private static decimal GetTotalDeposits(ICollectionsDB db) 
            => db.BankDeposits.GetAll().Sum(_ => _.Amount);
    }
}
