using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerformanceRow : List<CollectorPerfSubRow>
    {
        public CollectorPerformanceRow(CollectorDTO collector, ICollectionsDB db, IStallsRepo fallbackStallsRepo)
        {
            Collector = collector;

            foreach (var sec in db.SectionsSnapshot)
                AddSubRows(collector, sec, db, fallbackStallsRepo);

            Sections = this.GroupBy(_ => _.Section.Id)
                           .Select (_ => _.First())
                           .Select (_ => _.Section)
                           .ToList ();
        }


        public CollectorDTO       Collector  { get; }
        public List<SectionDTO>   Sections   { get; }

        public string Assignment => string.Join(", ", Sections?.Select(_ => _.Name));


        //private List<SectionDTO> GetSections(CollectorDTO collector, ICollectionsDB db)
        //    => db.SectionsSnapshot.Where(sec 
        //        => db.GetCollector(sec).Id == collector.Id).ToList();


        private void AddSubRows(CollectorDTO collector, SectionDTO sec, ICollectionsDB db, IStallsRepo stalls)
        {
            if (db.GetCollector(sec).Id != collector.Id) return;
            if (!db.IntendedColxns.TryGetValue(sec.Id, out IIntendedColxnsRepo repo)) return;
            if (!repo.Any()) return;

            foreach (var colxn in repo.GetAll())
            {
                if (colxn.StallSnapshot == null)
                    colxn.StallSnapshot = stalls.Find(colxn.Lease.Stall.Id, true);

                this.Add(new CollectorPerfSubRow(colxn, sec));
            }
        }
    }
}
