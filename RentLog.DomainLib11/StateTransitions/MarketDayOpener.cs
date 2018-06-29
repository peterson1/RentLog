using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.StateTransitions
{
    public class MarketDayOpener
    {
        public static List<Action> GetActions(ICollectionsDB colxnsDB, ITenantDBsDir dir)
        {
            var jobs    = new List<Action>();
            var balancd = colxnsDB.Date.AddDays(-1);

            EnqueueBalanceUpdates(jobs, balancd, dir);

            jobs.Add(() => AddDepositsToPassbook(balancd, dir));
            jobs.Add(() => colxnsDB.MarkAsOpened());
            return jobs;
        }


        private static void EnqueueBalanceUpdates(List<Action> jobs, DateTime balancd, ITenantDBsDir dir)
        {
            var activs = dir.MarketState.ActiveLeases.GetAll();

            foreach (var lse in activs)
                jobs.Add(() => dir.Balances.GetRepo(lse)
                                  .ProcessBalancedDay(balancd));
        }


        private static void AddDepositsToPassbook(DateTime colxnDate, ITenantDBsDir dir)
        {
            var db = dir.Collections.For(colxnDate);
            if (db == null) return;

            var deps = db.BankDeposits.GetAll();
            foreach (var dep in deps)
                dir.Passbooks.GetRepo(dep.BankAccount.Id)
                    .InsertDepositedColxn(dep, colxnDate);

            foreach (var id in deps.Select(_ => _.BankAccount.Id))
                dir.Passbooks.GetRepo(id).RecomputeBalancesFrom(colxnDate);
        }
    }
}
