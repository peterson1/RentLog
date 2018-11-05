using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class CollectionsDB1 : ICollectionsDB
    {
        private const string IS_OPENED     = "IsOpened";
        private const string POST_DATE     = "PostDate";
        private const string DATE_FMT      = "yyyy-MM-dd";
        private const string COLLECTOR_KEY = "Section{0}_CollectorID";
        private const string SEC_SNAPS_KEY = "SectionSnapshots";
        private const string COL_SNAPS_KEY = "CollectorSnapshots";

        private MarketStateDB  _mkt;
        private Dictionary<int, CollectorDTO> _collectorBySecID = new Dictionary<int, CollectorDTO>();


        public CollectionsDB1(DateTime date, IKeyValueStore metadataRepo, MarketStateDB marketStateDB, string databasePath)
        {
            Meta               = metadataRepo;
            _mkt               = marketStateDB;
            DatabasePath       = databasePath;
            Date               = date;
            SectionsSnapshot   = LoadSnapshot<SectionDTO>(SEC_SNAPS_KEY);
            CollectorsSnapshot = LoadSnapshot<CollectorDTO>(COL_SNAPS_KEY);
        }


        public Dictionary<int, IIntendedColxnsRepo> IntendedColxns { get; } = new Dictionary<int, IIntendedColxnsRepo>();
        public Dictionary<int, IAmbulantColxnsRepo> AmbulantColxns { get; } = new Dictionary<int, IAmbulantColxnsRepo>();
        public Dictionary<int, IUncollectedsRepo  > Uncollecteds   { get; } = new Dictionary<int, IUncollectedsRepo  >();
        public Dictionary<int, INoOperationsRepo  > NoOperations   { get; } = new Dictionary<int, INoOperationsRepo  >();
        public Dictionary<int, IVacantStallsRepo  > VacantStalls   { get; } = new Dictionary<int, IVacantStallsRepo  >();


        public string                   DatabasePath        { get; }
        public DateTime                 Date                { get; }
        public IKeyValueStore           Meta                { get; }
        public List<SectionDTO>         SectionsSnapshot    { get; }
        public List<CollectorDTO>       CollectorsSnapshot  { get; }
        public ICashierColxnsRepo       CashierColxns       { get; set; }
        public IOtherColxnsRepo         OtherColxns         { get; set; }
        public IBankDepositsRepo        BankDeposits        { get; set; }
        public IBalanceAdjustmentsRepo  BalanceAdjs         { get; set; }


        public CollectorDTO GetCollector(SectionDTO sec)
        {
            if (_collectorBySecID.TryGetValue(sec.Id, out CollectorDTO cachd))
                return cachd;

            var val = Meta[string.Format(COLLECTOR_KEY, sec.Id)];
            if (val.IsBlank()) return null;

            if (!int.TryParse(val, out int id))
                throw Fault.BadCast(val, id);

            cachd = _mkt.Collectors.Find(id, true);
            _collectorBySecID.Add(sec.Id, cachd);
            return cachd;
        }


        public CollectorDTO GetCollector(LeaseDTO lease)
        {
            if (lease.Stall.Section == null)
                _mkt.RefreshStall(lease);
            return GetCollector(lease.Stall.Section);
        }


        public void SetCollector(int sectionId, int collectorId)
        {
            var key = string.Format(COLLECTOR_KEY, sectionId);
            Meta[key] = collectorId.ToString();
        }


        public bool   IsPosted     () => Meta.Has(POST_DATE);
        public string PostedBy     () => Meta[POST_DATE];
        public bool   IsOpened     () => Meta.IsTrue(IS_OPENED);
        public void   MarkAsOpened () => Meta.SetTrue(IS_OPENED);

        public void MarkAsPosted(string postedBy)
            => Meta[POST_DATE] = postedBy;


        private List<T> LoadSnapshot<T>(string metaKey)
        {
            var json = Meta[metaKey];
            if (json.IsBlank()) return null;
            return json.ReadJson<List<T>>();
        }


        public void TakeSectionsSnapshot(List<SectionDTO> currentSections)
            => Meta[SEC_SNAPS_KEY] = currentSections.ToJson();


        public void TakeCollectorsSnapshot(List<CollectorDTO> currentCollectors)
            => Meta[COL_SNAPS_KEY] = currentCollectors.ToJson();


        public bool HasVacantsTable(SectionDTO sec)
        {
            if (!VacantStalls.TryGetValue(sec.Id, out IVacantStallsRepo repo)) return false;
            return repo.TableExists();
        }
    }
}
