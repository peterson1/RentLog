using RentLog.DomainLib11.DataSources;
using System;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class NonExpiringLeasesForSome
    {
        public static void Run(ITenantDBsDir dir)
        {
            var repo = dir.MarketState.ActiveLeases;
            var lses = repo.GetAll();

            foreach (var lse in lses)
            {
                if (lse.Rights.TotalAmount == 7_000)
                    lse.ContractEnd = DateTime.MaxValue;
            }

            repo.DropAndInsert(lses, true);
        }
    }
}
