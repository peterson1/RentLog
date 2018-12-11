using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using System;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class EditMarketMeta
    {
        public static Action SetYearsBack_1(ITenantDBsDir dir,
            out string jobDesc, out bool canRun)
        {
            canRun  = dir.CanRunAdHocTask(false);
            jobDesc = "Set YearsBackCount to [1]";
            return () =>
            {
                dir.MarketState.YearsBackCount = 1;
            };
        }
    }
}
