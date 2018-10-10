using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.ReflectionTools;
using RentLog.DomainLib11.BalanceRepos;
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

            var inactv = new InactiveLeaseDTO(lse, reason, deactivationDate, mkt.CurrentUser);
            mkt.InactiveLeases.Insert(inactv);

            return inactv;
        }


        public static void UndoLeaseTermination(this MarketStateDB mkt, InactiveLeaseDTO inactiveLeaseDTO)
        {
            //todo: reject if stall is in use
            var activ = new LeaseDTO();
            activ.CopyByNameFrom(inactiveLeaseDTO as LeaseDTO);
            mkt.ActiveLeases.Insert(activ);
            mkt.InactiveLeases.Delete(inactiveLeaseDTO);
        }
    }
}
