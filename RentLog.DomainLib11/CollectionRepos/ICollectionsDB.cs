using CommonTools.Lib11.DatabaseTools;
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

        IKeyValueStore           Meta                { get; }
        string                   DatabasePath        { get; }
        DateTime                 Date                { get; }
        ICashierColxnsRepo       CashierColxns       { get; }
        IOtherColxnsRepo         OtherColxns         { get; }
        IBankDepositsRepo        BankDeposits        { get; }
        IBalanceAdjustmentsRepo  BalanceAdjs         { get; }
        List<SectionDTO>         SectionsSnapshot    { get; }
        List<CollectorDTO>       CollectorsSnapshot  { get; }

        bool          IsPosted               ();
        string        PostedBy               ();
        bool          IsOpened               ();
        void          MarkAsOpened           ();
        void          MarkAsPosted           (string postedBy);
        CollectorDTO  GetCollector           (LeaseDTO lease);
        CollectorDTO  GetCollector           (SectionDTO section);
        void          SetCollector           (int sectionId, int collectorId);
        void          TakeSectionsSnapshot   (List<SectionDTO> currentSections);
        void          TakeCollectorsSnapshot (List<CollectorDTO> currentCollectors);
        bool          HasVacantsTable        (SectionDTO section);
    }
}
