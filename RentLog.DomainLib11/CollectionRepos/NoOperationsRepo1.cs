using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class NoOperationsRepo1 : SimpleRepoShimBase<UncollectedLeaseDTO>, INoOperationsRepo
    {
        public NoOperationsRepo1(ISimpleRepo<UncollectedLeaseDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
