using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.PassbookRepos;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.AdHocJobs
{
    public static class BankTxnsFix
    {
        public static Action RemoveDuplicates(int bankAcctId, ITenantDBsDir dir, out string jobDesc)
        {
            jobDesc   = "BankTxnsFix.RemoveDuplicates";
            return () =>
            {
                var repo  = dir.Passbooks.GetRepo(bankAcctId);
                var start = 1.Jan(2015);
                var end   = DateTime.Now.Date;

                foreach (var date in start.EachDayUpTo(end))
                    repo.RemoveDuplicatesFor(date);
            };
        }


        private static void RemoveDuplicatesFor (this IPassbookRowsRepo repo, DateTime date)
        {
            var hSet = new HashSet<int>();
            var rows = repo.RowsFor(date);
            foreach (var row in rows)
            {
                var hash = row.GetHashCode();
                if (hSet.Contains(hash))
                    repo.Delete(row);
                else
                    hSet.Add(hash);
            }
        }
    }
}
