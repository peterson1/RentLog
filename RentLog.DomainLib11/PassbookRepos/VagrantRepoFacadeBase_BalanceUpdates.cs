using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.PassbookRepos
{
    public abstract partial class VagrantRepoFacadeBase
    {
        public void RecomputeBalancesFrom(DateTime startDate)
        {
            var startBal = GetPreviousShardBalance(startDate);
            var dbPaths  = GetUniqueDBPaths(startDate, DateTime.Now);
            foreach (var path in dbPaths)
            {
                var db   = ConnectToDB(path);
                var rows = db.GetAll();
                UpdateRunningBalances(rows, startBal);

                db.DropAndInsert(rows, true, true);

                if (rows.Any())
                    startBal = rows.Last().RunningBalance;
            }
        }


        private void UpdateRunningBalances(List<PassbookRowDTO> rows, decimal startBal)
        {
            foreach (var row in rows)
            {
                row.RunningBalance = startBal + row.Amount;
                startBal           = row.RunningBalance;
            }
        }


        private decimal GetPreviousShardBalance(DateTime date)
        {
            var db = ConnectToDB(GetPreviousShardDBPath(date));
            if (!db.Any()) return 0;
            return db.GetAll().Last().RunningBalance;
        }
    }
}
