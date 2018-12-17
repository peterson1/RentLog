using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.ReportRows;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class CollectorsPerformanceReport : UIList<CollectorPerformanceRow>
    {
        public static CollectorsPerformanceReport New(IMarketStateDB mkt, ICollectionsDB db)
        {
            var cp = new CollectorsPerformanceReport();

            var collectors = db.CollectorsSnapshot
                          ?? mkt.Collectors.GetAll();

            foreach (var collector in collectors)
                cp.Add(CollectorPerformanceRow.New(collector, mkt.Stalls, db, mkt));

            cp.RemoveAll(_ => !_.Any());

            return cp;
        }
    }
}
