using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public abstract class CollectionsDirBase : ICollectionsDir
    {
        public IOrderedEnumerable<DateTime> AllDates()
            => FindAllDates().OrderBy(_ => _);
            

        public    abstract ICollectionsDB         For           (DateTime dateTime, bool createIfMissing);
        protected abstract IEnumerable<DateTime>  FindAllDates  ();


        public DateTime LastPostedDate()
        {
            var all = AllDates();
            if (!all.Any()) return DateTime.Now.AddDays(-1).Date;
            var dates = all.ToList();

            for (int i = dates.Count - 1; i >= 0; i--)
            {
                var repo = For(dates[i], false);
                if (repo.IsPosted()) return dates[i];
            }

            return dates.First().AddDays(-1);
        }


        public DateTime UnclosedDate() => LastPostedDate().AddDays(1);
    }
}

