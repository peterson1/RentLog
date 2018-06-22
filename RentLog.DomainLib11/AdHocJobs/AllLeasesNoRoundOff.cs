using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.DataSources;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class AllLeasesNoRoundOff
    {
        public static void Run(ITenantDBsDir dir)
        {
            var repo = dir.MarketState.ActiveLeases;
            var lses = repo.GetAll();

            foreach (var lse in lses)
                lse.Rent.PenaltyRule = RentPenalty.DailySurcharge_NoRoundOff;

            repo.DropAndInsert(lses, true);
        }
    }
}
