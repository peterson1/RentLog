using RentLog.DomainLib11.DataSources;
using System;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class ForSpecificLease
    {
        public static void RebuildSoaFrom(DateTime minDate, int leaseId, ITenantDBsDir dir)
            => dir.Balances.GetRepo(leaseId).UpdateFrom(minDate);
    }
}
