using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules.RentPenalties
{
    public abstract class RentPenaltyRuleBase : BillSurchargerBase
    {
        protected override void ValidateRuleName(LeaseDTO lse)
        {
            if (lse?.Rent == null)
                throw Null.Ref("Lease Rent Params");

            if (lse.Rent.PenaltyRule != RuleName)
                throw Bad.Key<LeaseDTO>(RuleName,
                    lse.Rent.PenaltyRule,
                    nameof(lse.Rent.PenaltyRule));
        }
    }
}
