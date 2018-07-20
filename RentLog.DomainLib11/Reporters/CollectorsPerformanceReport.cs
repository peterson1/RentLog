﻿using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.ReportRows;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class CollectorsPerformanceReport : List<CollectorPerformanceRow>
    {
        public CollectorsPerformanceReport(ICollectionsDB db, MarketStateDB mkt)
        {
            foreach (var collector in db.CollectorsSnapshot)
                this.Add(new CollectorPerformanceRow(collector, db, mkt.Stalls));

            this.RemoveAll(_ => !_.Any());
        }
    }
}
