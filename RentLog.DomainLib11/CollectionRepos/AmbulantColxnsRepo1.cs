using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class AmbulantColxnsRepo1 : SimpleRepoShimBase<AmbulantColxnDTO>, IAmbulantColxnsRepo
    {
        public AmbulantColxnsRepo1(ISimpleRepo<AmbulantColxnDTO> simpleRepo) : base(simpleRepo)
        {
        }


        protected override string GetWhyInvalid(AmbulantColxnDTO dto)
        {
            if (!dto.PRNumber.HasValue)
                return "PR # should not be blank";

            if (dto.Amount <= 0)
                return "Amount should be greater than zero.";

            if (dto.ReceivedFrom.IsBlank())
                return "“Received From” should not be blank";

            return string.Empty;
        }
    }
}
