using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerformanceRow
    {
        public CollectorPerformanceRow(CollectorDTO collector, ICollectionsDB db)
        {
            Collector = collector;
            Sections  = GetSections(collector, db);
        }


        public CollectorDTO       Collector  { get; }
        public List<SectionDTO>   Sections   { get; }

        public string Assignment => string.Join(", ", Sections?.Select(_ => _.Name));


        private List<SectionDTO> GetSections(CollectorDTO collector, ICollectionsDB db)
            => db.SectionsSnapshot.Where(sec 
                => db.GetCollector(sec).Id == collector.Id).ToList();
    }
}
