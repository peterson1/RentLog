using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.StateTransitions
{
    public static class LeaseIsActiveRules
    {
        public static bool IsActive(this LeaseDTO lse, DateTime asOfDate)
        {
            if (lse is InactiveLeaseDTO inactvLse) return false;
            //todo: lease inactivity rules
            return true;
        }
    }
}
