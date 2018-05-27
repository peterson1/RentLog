using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class CollectionsDB1 : ICollectionsDB
    {
        private const string POST_DATE     = "PostDate";
        private const string COLLECTOR_KEY = "Section{0}_CollectorID";

        private ISimpleRepo<KeyValuePairDTO> _meta;
        private MarketStateDB                _mkt;


        public CollectionsDB1(ISimpleRepo<KeyValuePairDTO> metadataRepo, MarketStateDB marketStateDB)
        {
            _meta = metadataRepo;
            _mkt  = marketStateDB;
        }


        public Dictionary<int, IIntendedColxnsRepo> IntendedColxns { get; } = new Dictionary<int, IIntendedColxnsRepo>();
        public Dictionary<int, IAmbulantColxnsRepo> AmbulantColxns { get; } = new Dictionary<int, IAmbulantColxnsRepo>();
        public Dictionary<int, IUncollectedsRepo  > Uncollecteds   { get; } = new Dictionary<int, IUncollectedsRepo  >();
        public Dictionary<int, INoOperationsRepo  > NoOperations   { get; } = new Dictionary<int, INoOperationsRepo  >();

        public ICashierColxnsRepo       CashierColxns   { get; set; }
        public IOtherColxnsRepo         OtherColxns     { get; set; }
        public IBankDepositsRepo        BankDeposits    { get; set; }
        public IBalanceAdjustmentsRepo  BalanceAdjs     { get; set; }


        public CollectorDTO GetCollector(LeaseDTO lease)
        {
            _mkt.RefreshStall(lease);

            var sec = lease.Stall.Section;
            var key = string.Format(COLLECTOR_KEY, sec.Id);
            var dto = _meta.Find(_ => _.Name == key);

            if (!dto.Any())
                throw RecordNotFoundException.For<KeyValuePairDTO>("Name", key);

            var idText = dto.Single().Value;

            if (!int.TryParse(idText, out int id))
                throw Fault.BadCast(idText, id);

            return _mkt.Collectors.Find(id, true);
        }


        public bool IsPosted() => _meta.HasName(POST_DATE);
    }
}
