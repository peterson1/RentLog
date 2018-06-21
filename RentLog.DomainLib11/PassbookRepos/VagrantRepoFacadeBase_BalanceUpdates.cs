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
            var startBal = GetOpeningBalance(startDate);
            var dbPaths  = GetUniqueDBPaths(startDate, DateTime.Now);
            foreach (var path in dbPaths)
            {
                var db   = ConnectToDB(path);
                var rows = db.GetAll();
                UpdateRunningBalances(rows, startBal);

                db.DropAndInsert(rows, true);

                if (rows.Any())
                    startBal = rows.LastOrDefault()?.RunningBalance ?? 0;
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


        private decimal GetOpeningBalance(DateTime date)
        {
            var row = RowsFor(date)?.FirstOrDefault();
            if (row == null) return 0;
            return row.RunningBalance - row.Amount;
        }
    }
}
