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
            {
                var row = new StallsInventoryRow(sec, colxns);
                if (row.TotalCount > 0)
                    this.Add(row);
            }

            this.SetSummary(new StallsInventoryTotal(this));
        }
    }
}
