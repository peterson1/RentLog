using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;

namespace RentLog.DomainLib11.StateTransitions
{
    public static class LeaseDeactivator
    {
        public static InactiveLeaseDTO DeactivateLease(this MarketStateDB mkt, LeaseDTO lse,
            string reason, DateTime deactivationDate)
        {
            if (lse is InactiveLeaseDTO)
                throw Bad.State<LeaseDTO>("Inactive", "Active");

            //mkt.ActiveLeases.Find(lse.Id, true);
            var inactv = new InactiveLeaseDTO(lse, reason, deactivationDate, mkt.CurrentUser);
            mkt.InactiveLeases.Insert(inactv);

            //todo: move this to InactivesRepo.AfterSave()
            mkt.ActiveLeases.Delete(lse);

            if (mkt.ActiveLeases.HasId(lse.Id))
                throw Bad.State<LeaseDTO>("Deactivated", "Exists-As-Active");

            return inactv;
        }
    }
}
