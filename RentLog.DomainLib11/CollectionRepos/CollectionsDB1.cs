using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class CollectionsDB1 : ICollectionsDB
    {
        private const string POST_DATE = "PostDate";
        private ISimpleRepo<KeyValuePairDTO> _meta;

        public CollectionsDB1(ISimpleRepo<KeyValuePairDTO> metadataRepo)
        {
            _meta = metadataRepo;
        }


        public ICashierColxnsRepo       CashierColxns  { get; set; }
        public IBalanceAdjustmentsRepo  BalanceAdjs    { get; set; }


        public bool IsPosted() => _meta.HasName(POST_DATE);
    }
}
