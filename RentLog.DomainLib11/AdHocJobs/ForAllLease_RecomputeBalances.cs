using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.AdHocJobs
{
    public partial class ForAllLeases
    {
        public static List<Action> RecomputeAllBalances(ITenantDBsDir dir)
        {
            var mkt  = dir.MarketState;
            var jobs = new List<Action>();

            foreach (var lse in mkt.ActiveLeases.GetAll())
                jobs.Add(() => RecomputeBalances(lse, dir));

            foreach (var lse in mkt.InactiveLeases.GetAll())
                jobs.Add(() => RecomputeBalances(lse, dir));

            return jobs;
        }


        private static void RecomputeBalances(LeaseDTO lse, ITenantDBsDir dir)
        {
                dir.Balances.GetRepo(lse)
                    .RecomputeAll();
        }
    }
}
