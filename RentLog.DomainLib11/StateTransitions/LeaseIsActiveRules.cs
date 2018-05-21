using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.StateTransitions
{
    public static class LeaseIsActiveRules
    {
        public static bool IsActive(this LeaseDTO lse, DateTime asOfDate)
        {
            if (lse is InactiveLeaseDTO inactv)
            {
                if (asOfDate > inactv.DeactivatedDate) return false;
            }

            if (lse.ContractStart > asOfDate) return false;
            if (lse.ContractEnd   < asOfDate) return false;

            if (lse.Stall == null) throw Fault.NullRef("Lease.Stall");
            if (!lse.Stall.IsOperational) return false;

            return true;
        }
    }
}
