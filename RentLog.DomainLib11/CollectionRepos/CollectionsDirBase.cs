using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public abstract class CollectionsDirBase : ICollectionsDir
    {
        public IOrderedEnumerable<DateTime> AllDates()
            => FindAllDates().OrderBy(_ => _);
            

        protected abstract ICollectionsDB GetDB(DateTime dateTime);
        protected abstract IEnumerable<DateTime> FindAllDates();


        public DateTime LastPostedDate()
        {
            var dates = AllDates().ToList();

            for (int i = dates.Count - 1; i >= 0; i--)
            {
                var repo = GetDB(dates[i]);
                if (repo.IsPosted()) return dates[i];
            }

            return dates.First().AddDays(-1);
        }
    }
}
