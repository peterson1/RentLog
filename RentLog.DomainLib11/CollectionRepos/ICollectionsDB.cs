using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface ICollectionsDB
    {
        Dictionary<int, IIntendedColxnsRepo> IntendedColxns { get; }
        Dictionary<int, IAmbulantColxnsRepo> AmbulantColxns { get; }
        Dictionary<int, IUncollectedsRepo>   Uncollecteds   { get; }
        Dictionary<int, INoOperationsRepo>   NoOperations   { get; }
        Dictionary<int, IVacantStallsRepo>   VacantStalls   { get; }

        DateTime                 Date            { get; }
        ICashierColxnsRepo       CashierColxns   { get; }
        IOtherColxnsRepo         OtherColxns     { get; }
        IBankDepositsRepo        BankDeposits    { get; }
        IBalanceAdjustmentsRepo  BalanceAdjs     { get; }

        bool          IsPosted      ();
        bool          IsOpened      ();
        void          MarkAsOpened  ();
        void          MarkAsPosted  ();
        CollectorDTO  GetCollector  (LeaseDTO lease);
        CollectorDTO  GetCollector  (SectionDTO section);
        void          SetCollector  (SectionDTO section, CollectorDTO collector);
    }
}
