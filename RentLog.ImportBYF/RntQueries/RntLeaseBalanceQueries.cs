using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Version2UI;
using System;

namespace RentLog.ImportBYF.RntQueries
{
    public static class RntLeaseBalanceQueries
    {
        public static BillAmounts GetBalances(this MainWindowVM2 main, LeaseDTO lse)
        {
            var dir       = main.AppArgs;
            var repo      = dir.Balances.GetRepo(lse.Id);
            var lastPostd = main.ByfServer.LastPostedDate;
            var rowId     = lastPostd.DaysSinceMin();
            var row       = repo.Find(rowId, false);
            if (row == null) return null;
            return new BillAmounts
            {
                Rent   = row.For(BillCode.Rent  )?.ClosingBalance,
                Rights = row.For(BillCode.Rights)?.ClosingBalance,
            };
        }
    }
}
