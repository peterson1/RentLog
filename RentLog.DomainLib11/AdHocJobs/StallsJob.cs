using RentLog.DomainLib11.DataSources;
using System.Linq;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class StallsJob
    {
        public static void RenameOldStalls(ITenantDBsDir dir)
        {
            var repo      = dir.MarketState.Stalls;
            var allStalls = repo.GetAll();

            foreach (var stall in allStalls)
            {
                if (stall.Section.Id <= 3)
                    stall.Name += " (old)";
            }

            repo.DropAndInsert(allStalls, true, false);
        }
    }
}
