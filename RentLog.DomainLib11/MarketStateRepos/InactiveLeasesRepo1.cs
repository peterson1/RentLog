using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class InactiveLeasesRepo1 : MarketStateRepoShimBase<InactiveLeaseDTO>, IInactiveLeasesRepo
    {
        public InactiveLeasesRepo1(ISimpleRepo<InactiveLeaseDTO> simpleRepo, MarketStateDB marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }


        public List<InactiveLeaseDTO> BySection(int sectionId)
            => _repo.Find(_ => _.SectionID == sectionId);


        protected override void ValidateBeforeInsert(InactiveLeaseDTO lse)
        {
            if (!_db.ActiveLeases.HasId(lse.Id))
                throw Bad.Insert(lse, "Missing from Actives");
        }


        protected override void ExecuteAfterSave(InactiveLeaseDTO lse, bool operationIsDelete)
        {
            if (operationIsDelete) return;

            _db.ActiveLeases.Delete(lse.Id);

            if (_db.ActiveLeases.HasId(lse.Id))
                throw Bad.State<LeaseDTO>("Deactivated", "Exists-As-Active");

            _db.Balances.GetRepo(lse.Id)
               .RecomputeFrom(lse.DeactivatedDate);
        }
    }
}
