using System;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface ICollectionsDir
    {
        DateTime LastPostedDate();
        IOrderedEnumerable<DateTime> AllDates();
    }
}
