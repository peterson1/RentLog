using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class InactiveLeasesRepo1 : MarketStateRepoShimBase<InactiveLeaseDTO>, IInactiveLeasesRepo
    {
        public InactiveLeasesRepo1(ISimpleRepo<InactiveLeaseDTO> simpleRepo, MarketStateDB marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }


        protected override void ValidateBeforeInsert(InactiveLeaseDTO lse)
        {
            if (!_db.ActiveLeases.HasId(lse.Id))
                throw Bad.Insert(lse, "Missing from Actives");
        }


        protected override void ExecuteAfterSave(InactiveLeaseDTO record)
        {
            //todo: delete record from Actives
        }
    }
}
