using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class InactiveLeasesRepo1 : MarketStateRepoShimBase<InactiveLeaseDTO>, IInactiveLeasesRepo
    {
        public InactiveLeasesRepo1(ISimpleRepo<InactiveLeaseDTO> simpleRepo, MarketStateDB marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }
    }
}
