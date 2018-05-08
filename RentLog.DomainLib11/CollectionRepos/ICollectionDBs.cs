using System;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface ICollectionDBs
    {
        DateTime LastPostedDate();
        IOrderedEnumerable<DateTime> AllDates();
    }
}
