using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.StateTransitions
{
    public class MarketDayCloser
    {
        public static List<Action> GetActions(ICollectionsDB colxnsDB, ITenantDBsDir dir, string postedBy)
        {
            var jobs = new List<Action>();

            jobs.Add(() => colxnsDB.MarkAsPosted(postedBy));

            return jobs;
        }
    }
}
