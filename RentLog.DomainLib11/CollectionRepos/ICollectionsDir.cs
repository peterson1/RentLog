using System;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface ICollectionsDir
    {
        DateTime                      LastPostedDate ();
        DateTime                      UnclosedDate   ();
        IOrderedEnumerable<DateTime>  AllDates       ();
        ICollectionsDB                For            (DateTime date);
        ICollectionsDB                CreateFor      (DateTime date);
    }
}
