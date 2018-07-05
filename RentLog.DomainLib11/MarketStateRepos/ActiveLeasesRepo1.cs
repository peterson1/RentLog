using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
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


        protected override string GetWhyInvalid(LeaseDTO dto)
        {
            if (dto.Tenant == null)
                return $"‹{typeof(TenantModel).Name}› should not be NULL.";

            if (dto.Tenant.FirstName.IsBlank())
                return $"‹First Name› should not be blank.";

            if (dto.Tenant.MiddleName.IsBlank())
                return $"‹Middle Name› should not be blank.";

            if (dto.Tenant.LastName.IsBlank())
                return $"‹Last Name› should not be blank.";

            if (dto.Tenant.BirthDate == DateTime.MinValue)
                return $"‹Birth Date› should not be blank.";

            if (dto.Tenant.Phone1.IsBlank())
                return $"‹Primary Phone Number› should not be blank.";

            if (dto.Tenant.LotNumber.IsBlank())
                return $"‹Lot #› should not be blank.";

            if (dto.Tenant.StreetName.IsBlank())
                return $"‹Street Name› should not be blank.";

            if (dto.Tenant.Barangay.IsBlank())
                return $"‹Barangay› should not be blank.";

            if (dto.Tenant.Municipality.IsBlank())
                return $"‹Municipality› should not be blank.";

            if (dto.Tenant.Province.IsBlank())
                return $"‹Province› should not be blank.";

            if (dto.ProductToSell.IsBlank())
                return $"‹Product-to-Sell› should not be blank.";

            if (dto.ContractStart == DateTime.MinValue)
                return $"‹Contract-Start› should not be blank.";

            if (dto.ContractEnd < dto.ContractStart)
                return $"‹Contract-End› should be later than ‹Contract-Start›.";

            if (StallConflictsWithAnotherLease(dto))
                return $"Stall “{dto.Stall}” is already occupied.";

            return string.Empty;
        }


        private bool StallConflictsWithAnotherLease(LeaseDTO lse)
        {
            if (!Any()) return false;

            var occupieds = GetAll().Select(_ => _.Stall.Id).ToList();

            if (lse.Id > 0)
            {
                var orig = _repo.Find(lse.Id, true);
                occupieds.Remove(orig.Stall.Id);
            }

            return occupieds.Contains(lse.Stall.Id);
        }

        protected override void ExecuteAfterSave(LeaseDTO lse, bool operationIsDelete)
        {
            if (operationIsDelete) return;
            var stallRec           = _db.Stalls.Find(lse.Stall.Id, true);
            stallRec.DefaultRent   = lse.Rent;
            stallRec.DefaultRights = lse.Rights;
            _db.Stalls.Update(stallRec);
        }
    }
}
