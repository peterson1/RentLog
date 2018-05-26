using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class AmbulantColxnsRepo1 : SimpleRepoShimBase<AmbulantColxnDTO>, IAmbulantColxnsRepo
    {
        public AmbulantColxnsRepo1(ISimpleRepo<AmbulantColxnDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
