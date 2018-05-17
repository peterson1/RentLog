using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class IntendedColxnsRepo1 : SimpleRepoShimBase<IntendedColxnDTO>, IIntendedColxnsRepo
    {
        public IntendedColxnsRepo1(ISimpleRepo<IntendedColxnDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
