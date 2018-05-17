using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface ICollectionsDB
    {
        Dictionary<int, IIntendedColxnsRepo> IntendedColxns { get; }
        ICashierColxnsRepo       CashierColxns   { get; }
        IBalanceAdjustmentsRepo  BalanceAdjs     { get; }

        bool          IsPosted      ();
        CollectorDTO  GetCollector  (LeaseDTO lease);
    }
}
