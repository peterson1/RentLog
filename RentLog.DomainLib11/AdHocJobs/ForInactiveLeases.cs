using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class ForInactiveLeases
    {
        public static List<Action> RebuildSoA(ITenantDBsDir dir)
        {
            var jobs = new List<Action>();
            var lses = dir.MarketState.InactiveLeases.GetAll();

            foreach (var lse in lses)
            {
                jobs.Add(() =>
                    dir.Balances.GetRepo(lse).RecomputeAll());
            }
            return jobs;
        }
    }
}
