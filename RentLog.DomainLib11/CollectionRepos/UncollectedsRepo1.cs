using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class UncollectedsRepo1 : SimpleRepoShimBase<UncollectedLeaseDTO>, IUncollectedsRepo
    {
        public UncollectedsRepo1(ISimpleRepo<UncollectedLeaseDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
