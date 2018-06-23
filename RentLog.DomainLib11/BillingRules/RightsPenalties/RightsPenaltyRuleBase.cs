using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.BillingRules.RightsPenalties
{
    public abstract class RightsPenaltyRuleBase : BillSurchargerBase
    {
        protected override void ValidateRuleName(LeaseDTO lse)
        {
            if (lse?.Rights == null)
                throw Null.Ref("Lease Rights Params");

            if (lse.Rights.PenaltyRule != RuleName)
                throw Bad.Key<LeaseDTO>(RuleName,
                    lse.Rights.PenaltyRule,
                    nameof(lse.Rights.PenaltyRule));
        }
    }
}
