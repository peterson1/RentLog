using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public abstract class CollectionDBsBase : ICollectionDBs
    {
        public IOrderedEnumerable<DateTime> AllDates()
            => FindAllDates().OrderBy(_ => _);
            
            
        protected abstract IEnumerable<DateTime> FindAllDates();


        public DateTime LastPostedDate()
        {
            var dates = AllDates().ToList();

            for (int i = dates.Count - 1; i >= 0; i--)
                if (IsPosted(dates[i])) return dates[i];

            return dates.First().AddDays(-1);
        }


        private bool IsPosted(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
