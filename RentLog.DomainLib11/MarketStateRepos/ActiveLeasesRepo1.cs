using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class ActiveLeasesRepo1 : MarketStateRepoShimBase<LeaseDTO>, IActiveLeasesRepo
    {
        public ActiveLeasesRepo1(ISimpleRepo<LeaseDTO> simpleRepo, MarketStateDB allRepositories) : base(simpleRepo, allRepositories)
        {
        }


        public Dictionary<int, LeaseDTO> StallsLookup()
            => _repo.GetAll().ToDictionary(_ => _.Stall.Id);


        protected override void ValidateBeforeDelete(LeaseDTO lse)
        {
            if (!_db.InactiveLeases.HasId(lse.Id))
                throw Bad.Delete(lse, "Missing from Inactives");
        }
    }
}
