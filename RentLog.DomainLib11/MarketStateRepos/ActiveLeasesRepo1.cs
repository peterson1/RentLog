using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class ActiveLeasesRepo1 : MarketStateRepoShimBase<LeaseDTO>, IActiveLeasesRepo
    {
        public ActiveLeasesRepo1(ISimpleRepo<LeaseDTO> simpleRepo, IMarketStateDB allRepositories) : base(simpleRepo, allRepositories)
        {
        }


        public Dictionary<int, LeaseDTO> StallsLookup()
            => _repo.GetAll().ToDictionary(_ => _.Stall.Id);


        protected override void ValidateBeforeDelete(LeaseDTO lse)
        {
            if (!_db.InactiveLeases.HasId(lse.Id))
                throw Bad.Delete(lse, "Missing from Inactives");
        }


        public override bool IsValidForInsert(LeaseDTO draft, out string whyInvalid)
        {
            if (!base.IsValidForInsert(draft, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(draft);
            return whyInvalid.IsBlank();
        }


        public override bool IsValidForUpdate(LeaseDTO record, out string whyInvalid)
        {
            if (!base.IsValidForUpdate(record, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(record);
            return whyInvalid.IsBlank();
        }


        private string GetWhyInvalid(LeaseDTO dto)
        {
            if (dto.Tenant == null)
                return $"‹{typeof(TenantModel).Name}› should not be NULL.";

            if (dto.Tenant.FirstName.IsBlank())
                return $"‹First Name› should not be blank.";

            return string.Empty;
        }
    }
}
