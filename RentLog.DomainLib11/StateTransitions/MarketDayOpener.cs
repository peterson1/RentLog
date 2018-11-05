using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.StateTransitions
{
    public static class MarketDayOpener
    {
        public static List<Action> GetActions(ICollectionsDB colxnsDB, ITenantDBsDir dir)
        {
            var jobs    = new List<Action>();
            var balancd = colxnsDB.Date.AddDays(-1);

            EnqueueBalanceUpdates(jobs, balancd, dir);

            jobs.Add(() => dir.AddDepositsToPassbook(balancd));
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


        public static void AddDepositsToPassbook(this ITenantDBsDir dir, DateTime colxnDate)
        {
            var db = dir.Collections.For(colxnDate);
            if (db == null) return;

            var deps = db.BankDeposits.GetAll();
            dir.UpdateAccountPassbooks(deps, colxnDate);
        }


        public static void UpdateAccountPassbooks(this ITenantDBsDir dir, IEnumerable<BankDepositDTO> deps, DateTime colxnDate)
        {
            foreach (var dep in deps)
                dir.Passbooks.GetRepo(dep.BankAccount.Id)
                    .InsertDepositedColxn(dep, colxnDate);

            foreach (var id in deps.Select(_ => _.BankAccount.Id))
                dir.Passbooks.GetRepo(id).RecomputeBalancesFrom(colxnDate);
        }
    }
}
