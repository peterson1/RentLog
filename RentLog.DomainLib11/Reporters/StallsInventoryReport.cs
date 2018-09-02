using System;
using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.ReportRows;

namespace RentLog.DomainLib11.Reporters
{
    public class StallsInventoryReport : UIList<StallsInventoryRow>
    {
        public StallsInventoryReport(ICollectionsDB colxns, MarketStateDB mkt)
        {
            var sections = colxns.SectionsSnapshot ?? mkt.Sections.GetAll();

            foreach (var sec in sections)
                this.Add(new StallsInventoryRow(sec, colxns));

            this.SetSummary(new StallsInventoryTotal(this));
        }
    }
}
