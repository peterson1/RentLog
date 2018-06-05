using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public abstract class CollectionsDirBase : ICollectionsDir
    {
            

        protected abstract IEnumerable<DateTime>  FindAllDates  ();
        protected abstract ICollectionsDB         ConnectToDB   (DateTime date, string path);
        protected abstract bool                   TryFindDB     (DateTime date, out string path);


        public DateTime LastPostedDate()
        {
            var all = AllDates();
            if (!all.Any()) return DateTime.Now.AddDays(-1).Date;
            var dates = all.ToList();

            for (int i = dates.Count - 1; i >= 0; i--)
            {
                var repo = For(dates[i]);
                if (repo.IsPosted()) return dates[i];
            }

            return dates.First().AddDays(-1);
        }


        public ICollectionsDB For(DateTime date)
        {
            if (!TryFindDB(date, out string path)) return null;
            return ConnectToDB(date, path);
        }


        public ICollectionsDB CreateFor(DateTime date)
        {
            TryFindDB(date, out string file);
            return ConnectToDB(date, file);
        }


        public DateTime UnclosedDate() 
            => LastPostedDate().AddDays(1);


        public IOrderedEnumerable<DateTime> AllDates() 
            => FindAllDates().OrderBy(_ => _);
    }
}

