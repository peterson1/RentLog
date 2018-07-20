using RentLog.DomainLib11.CollectionRepos;

namespace RentLog.DomainLib11.ReportRows
{
    public class ColPerfStallCoverage
    {
        public ColPerfStallCoverage(CollectorPerformanceRow colPerfRow, ICollectionsDB db)
        {
            CollectedsCount = colPerfRow.Count;

            foreach (var sec in colPerfRow.Sections)
            {
                if (db.Uncollecteds.TryGetValue(sec.Id, out IUncollectedsRepo uncolRepo))
                    UncollectedsCount += uncolRepo.GetAll().Count;

                if (db.NoOperations.TryGetValue(sec.Id, out INoOperationsRepo noOpsRepo))
                    NoOperationsCount += noOpsRepo.GetAll().Count;
            }
        }


        public int       CollectedsCount     { get; }
        public int       UncollectedsCount   { get; }
        public int       NoOperationsCount   { get; }

        public int     ActiveStallsCount => UncollectedsCount + CollectedsCount;
        public decimal CoverageRate      => (decimal)CollectedsCount / (decimal) ActiveStallsCount;
    }
}
