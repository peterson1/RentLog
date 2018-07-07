using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.ReportRows;
using System.Collections.Generic;

namespace RentLog.DomainLib11.Reporters
{
    public class StallsInventoryReport : Dictionary<int, StallsInventoryRow>
    {
        public StallsInventoryReport(ICollectionsDB colxns, MarketStateDB mkt)
        {
            var sections = colxns.SectionsSnapshot ?? mkt.Sections.GetAll();

            foreach (var sec in sections)
                this.Add(sec.Id, new StallsInventoryRow(sec, colxns));
        }
    }
}
