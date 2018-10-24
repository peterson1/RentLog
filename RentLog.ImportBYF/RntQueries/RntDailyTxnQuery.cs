using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.ImportBYF.DailyTransactions;
using System;
using System.Linq;

namespace RentLog.ImportBYF.RntQueries
{
    public static class RntDailyTxnQuery
    {
        public static DailyTransactionCell QueryRNT(this ITenantDBsDir dir, DateTime date)
        {
            var db = dir.Collections.For(date);
            if (db == null) return new DailyTransactionCell();

            return new DailyTransactionCell
            {
                TotalCollections = GetIntendedColxnsTotal(db)
                                 + GetAmbulantColxnsTotal(db)
                                 + GetOtherColxnsTotal   (db)
                                 + GetCashierColxnsTotal (db),                //TotalCollections = intendeds + others + cashiers,
                TotalDeposits    = GetTotalDeposits(db),
            };
        }


        private static decimal GetIntendedColxnsTotal(ICollectionsDB db)
        {
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
            var all = db.OtherColxns.GetAll();
            var sum = all.Sum(_ => _.Amount);
            return sum;
        }

        private static decimal GetCashierColxnsTotal(ICollectionsDB db)
        {
            var all = db.CashierColxns.GetAll();
            var sum = all.Sum(_ => _.Amount);
            return sum;
        }

        private static decimal GetTotalDeposits(ICollectionsDB db) 
            => db.BankDeposits.GetAll().Sum(_ => _.Amount);
    }
}
