using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class CollectionsDB1 : ICollectionsDB
    {
        private const string POST_DATE = "PostDate";
        private ISimpleRepo<KeyValuePairDTO> _meta;

        public CollectionsDB1(ISimpleRepo<KeyValuePairDTO> metadataRepo, 
                              ICashierColxnsRepo cashierColxnsRepo,
                              IBalanceAdjustmentsRepo balanceAdjustmentsRepo)
        {
            _meta         = metadataRepo;
            CashierColxns = cashierColxnsRepo;
            BalanceAdjs   = balanceAdjustmentsRepo;
        }


        public ICashierColxnsRepo       CashierColxns  { get; }
        public IBalanceAdjustmentsRepo  BalanceAdjs    { get; }


        public bool IsPosted() => _meta.HasName(POST_DATE);
    }
}
