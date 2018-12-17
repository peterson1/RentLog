using RentLog.DomainLib11.CollectionRepos;

namespace RentLog.DomainLib11.ReportRows
{
    public class ColPerfStallCoverage
    {
        public int   CollectedsCount     { get; private set; }
        public int   UncollectedsCount   { get; private set; }
        public int   NoOperationsCount   { get; private set; }

        public int     ActiveStallsCount => UncollectedsCount + CollectedsCount;
        public decimal CoverageRate      => (decimal)CollectedsCount / (decimal) ActiveStallsCount;


        public static ColPerfStallCoverage New (CollectorPerformanceRow colPerfRow, ICollectionsDB db)
        {
            var sc = new ColPerfStallCoverage();
            sc.CollectedsCount = colPerfRow.Count;

            foreach (var sec in colPerfRow.Sections)
            {
                if (db.Uncollecteds.TryGetValue(sec.Id, out IUncollectedsRepo uncolRepo))
                    sc.UncollectedsCount += uncolRepo.GetAll().Count;

                if (db.NoOperations.TryGetValue(sec.Id, out INoOperationsRepo noOpsRepo))
                    sc.NoOperationsCount += noOpsRepo.GetAll().Count;
            }
            return sc;
        }
    }
}
