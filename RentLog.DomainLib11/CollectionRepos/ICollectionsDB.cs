using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface ICollectionsDB
    {
        Dictionary<int, IIntendedColxnsRepo> IntendedColxns { get; }
        ICashierColxnsRepo       CashierColxns   { get; }
        IOtherColxnsRepo         OtherColxns     { get; }
        IBankDepositsRepo        BankDeposits    { get; }
        IBalanceAdjustmentsRepo  BalanceAdjs     { get; }

        bool          IsPosted      ();
        CollectorDTO  GetCollector  (LeaseDTO lease);
    }
}
