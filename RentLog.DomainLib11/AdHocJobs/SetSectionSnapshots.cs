using RentLog.DomainLib11.DataSources;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class SetSectionSnapshots
    {
        public static void Run(ITenantDBsDir dir)
        {
            var current = dir.MarketState.Sections.GetAll();
            foreach (var date in dir.Collections.AllDates())
            {
                var db = dir.Collections.For(date);

                if (db.SectionsSnapshot == null)
                    db.TakeSectionsSnapshot(current);
            }
        }
    }
}
