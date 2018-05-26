using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class OtherColxnsRepo1 : SimpleRepoShimBase<OtherColxnDTO>, IOtherColxnsRepo
    {
        public OtherColxnsRepo1(ISimpleRepo<OtherColxnDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
