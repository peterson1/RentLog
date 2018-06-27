using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class IntendedColxnsRepo1 : SimpleRepoShimBase<IntendedColxnDTO>, IIntendedColxnsRepo
    {
        public IntendedColxnsRepo1(ISimpleRepo<IntendedColxnDTO> simpleRepo) : base(simpleRepo)
        {
        }


        protected override string GetWhyInvalid(IntendedColxnDTO dto)
        {
            if (dto.PRNumber <= 0)
                return "PR # should be greater than zero.";

            if (dto.Lease == null)
                return "Lease should not be NULL.";

            if (dto.Targets == null)
                return "Targets should not be NULL.";

            if (dto.Actuals == null)
                return "Actuals should not be NULL.";

            if (dto.Actuals.Total <= 0)
                return "Total should be greater than zero.";

            return string.Empty;
        }
    }
}
