using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.ReportRows;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class CollectorsPerformanceReport : UIList<CollectorPerformanceRow>
    {
        public CollectorsPerformanceReport(ICollectionsDB db, MarketStateDB mkt)
        {
            foreach (var collector in db.CollectorsSnapshot)
                this.Add(new CollectorPerformanceRow(collector, db, mkt.Stalls));

            this.RemoveAll(_ => !_.Any());
        }
    }
}
