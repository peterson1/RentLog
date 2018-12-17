using CommonTools.Lib11.CollectionTools;
using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerformanceRow : UIList<CollectorPerfSubRow>
    {
        private CollectorPerformanceRow(CollectorDTO collector)
        {
            Collector = collector;
        }

                                                     
        public CollectorDTO            Collector     { get; }
        public List<SectionDTO>        Sections      { get; private set; }
        public ColPerfStallCoverage    StallCoverage { get; private set; }
        public ColPerfBillPerformance  RentBill      { get; private set; }
        public ColPerfBillPerformance  RightsBill    { get; private set; }

        public string Assignment => string.Join(", ", Sections?.Select(_ => _.Name));


        private void AddSubRows(CollectorDTO collector, SectionDTO sec, ICollectionsDB db, IStallsRepo stalls)
        {
            var savedCollectr = db.GetCollector(sec)
                             ?? new CollectorDTO { Id = 1 };

            if (savedCollectr.Id != collector.Id) return;
            if (!db.IntendedColxns.TryGetValue(sec.Id, out IIntendedColxnsRepo repo)) return;
            if (!repo.Any()) return;

            foreach (var colxn in repo.GetAll())
            {
                if (colxn.StallSnapshot == null)
                    colxn.StallSnapshot = stalls.Find(colxn.Lease.Stall.Id, true);

                this.Add(new CollectorPerfSubRow(colxn, sec));
            }
        }


        public static CollectorPerformanceRow New (CollectorDTO collector, IStallsRepo fallbackStallsRepo, ICollectionsDB db, IMarketStateDB mkt)
        {
            var cp = new CollectorPerformanceRow(collector);
            var sections = db.SectionsSnapshot ?? mkt.Sections.GetAll();

            foreach (var sec in sections)
                cp.AddSubRows(collector, sec, db, fallbackStallsRepo);

            cp.Sections = cp.DistinctBy(_ => _.Section.Id)
                            .Select    (_ => _.Section)
                            .ToList    ();

            cp.StallCoverage = ColPerfStallCoverage  .New(cp, db);
            cp.RentBill      = ColPerfBillPerformance.New(BillCode.Rent  , cp.Select(_ => _.Rent));
            cp.RightsBill    = ColPerfBillPerformance.New(BillCode.Rights, cp.Select(_ => _.Rights));

            cp.SetSummary(new CollectorPerfSubRowsTotal(cp));

            return cp;
        }
    }
}
