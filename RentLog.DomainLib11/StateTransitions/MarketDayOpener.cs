using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.StateTransitions
{
    public class MarketDayOpener
    {
        public static List<Action> GetActions(ICollectionsDB colxnsDB, ITenantDBsDir dir)
        {
            var jobs    = new List<Action>();
            var balancd = colxnsDB.Date.AddDays(-1);
            var activs  = dir.MarketState.ActiveLeases.GetAll();

            foreach (var lse in activs)
                jobs.Add(() => dir.Balances.GetRepo(lse)
                                  .ProcessBalancedDay(balancd));

            jobs.Add(() => colxnsDB.MarkAsOpened());

            return jobs;
        }
    }
}
