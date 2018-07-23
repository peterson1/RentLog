using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private IKeyValueStore _meta;
        private MarketStateDB  _mkt;


        public CollectionsDB1(DateTime date, IKeyValueStore metadataRepo, MarketStateDB marketStateDB, string databasePath)
        {
            _meta              = metadataRepo;
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
        public List<SectionDTO>         SectionsSnapshot    { get; }
        public List<CollectorDTO>       CollectorsSnapshot  { get; }
        public ICashierColxnsRepo       CashierColxns       { get; set; }
        public IOtherColxnsRepo         OtherColxns         { get; set; }
        public IBankDepositsRepo        BankDeposits        { get; set; }
        public IBalanceAdjustmentsRepo  BalanceAdjs         { get; set; }


        public CollectorDTO GetCollector(SectionDTO sec)
        {
            var val = _meta[string.Format(COLLECTOR_KEY, sec.Id)];
            if (val.IsBlank()) return null;

            if (!int.TryParse(val, out int id))
                throw Fault.BadCast(val, id);

            return _mkt.Collectors.Find(id, true);
        }


        public CollectorDTO GetCollector(LeaseDTO lease)
        {
            _mkt.RefreshStall(lease);
            return GetCollector(lease.Stall.Section);
        }


        public void SetCollector(SectionDTO section, CollectorDTO collector)
        {
            var key = string.Format(COLLECTOR_KEY, section.Id);
            _meta[key] = collector.Id.ToString();
        }


        public bool IsPosted     () => _meta.Has(POST_DATE);
        public bool IsOpened     () => _meta.IsTrue(IS_OPENED);
        public void MarkAsOpened () => _meta.SetTrue(IS_OPENED);
        public void MarkAsPosted () => _meta[POST_DATE] = DateTime.Now.ToString(DATE_FMT);


        private List<T> LoadSnapshot<T>(string metaKey)
        {
            var json = _meta[metaKey];
            if (json.IsBlank()) return null;
            return json.ReadJson<List<T>>();
        }


        public void TakeSectionsSnapshot(List<SectionDTO> currentSections)
            => _meta[SEC_SNAPS_KEY] = currentSections.ToJson();


        public void TakeCollectorsSnapshot(List<CollectorDTO> currentCollectors)
            => _meta[COL_SNAPS_KEY] = currentCollectors.ToJson();


        public bool HasVacantsTable(SectionDTO sec)
        {
            if (!VacantStalls.TryGetValue(sec.Id, out IVacantStallsRepo repo)) return false;
            return repo.TableExists();
        }
    }
}
