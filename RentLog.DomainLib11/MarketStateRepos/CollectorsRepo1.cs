using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class CollectorsRepo1 : MarketStateRepoShimBase<CollectorDTO>, ICollectorsRepo
    {
        public CollectorsRepo1(ISimpleRepo<CollectorDTO> simpleRepo, MarketStateDB marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }


        protected override void ValidateBeforeInsert(CollectorDTO newRecord)
        {
            this.RejectDuplicateRecord(_ => _.Name == newRecord.Name,
                nameof(newRecord.Name), newRecord);
        }


        protected override void ValidateBeforeUpdate(CollectorDTO changedRecord)
        {
            this.RejectDuplicateRecord(_ => _.Name == changedRecord.Name,
                nameof(changedRecord.Name), changedRecord);
        }
    }
}
