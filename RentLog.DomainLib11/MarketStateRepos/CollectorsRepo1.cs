using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class CollectorsRepo1 : MarketStateRepoShimBase<CollectorDTO>, ICollectorsRepo
    {
        public CollectorsRepo1(ISimpleRepo<CollectorDTO> simpleRepo, MarketStateDbBase marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }

        protected override IEnumerable<CollectorDTO> ToSorted(IEnumerable<CollectorDTO> items)
            => items.OrderBy(_ => _.Name);


        protected override void ValidateBeforeInsert(CollectorDTO newRecord)
            => this.RejectDuplicateRecord(_ => _.Name == newRecord.Name,
                    nameof(newRecord.Name), newRecord);


        protected override void ValidateBeforeUpdate(CollectorDTO changedRecord)
            => this.RejectDuplicateRecord(_ => _.Name == changedRecord.Name,
                    nameof(changedRecord.Name), changedRecord, _ => _.Id);
    }
}
