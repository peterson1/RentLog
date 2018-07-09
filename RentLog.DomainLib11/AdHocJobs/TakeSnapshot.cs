using RentLog.DomainLib11.DataSources;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class TakeSnapshot
    {
        public static void OfSections(ITenantDBsDir dir)
        {
            var current = dir.MarketState.Sections.GetAll();
            foreach (var date in dir.Collections.AllDates())
            {
                var db = dir.Collections.For(date);

                if (db.SectionsSnapshot == null)
                    db.TakeSectionsSnapshot(current);
            }
        }


        public static void OfCollectors(ITenantDBsDir dir)
        {
            var current = dir.MarketState.Collectors.GetAll();
            foreach (var date in dir.Collections.AllDates())
            {
                var db = dir.Collections.For(date);

                if (db.CollectorsSnapshot == null)
                    db.TakeCollectorsSnapshot(current);
            }
        }
    }
}
