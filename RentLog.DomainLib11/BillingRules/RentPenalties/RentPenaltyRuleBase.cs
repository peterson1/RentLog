using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules.RentPenalties
{
    public struct RentPenalty
    {
        public const string DailySurcharge            = "Daily Surcharge";
        public const string DailySurcharge_NoRoundOff = "Daily Surcharge (No Round-off)";
    }


    public abstract class RentPenaltyRuleBase : IBillSurcharger
    {
        protected abstract List<BillPenalty> GetPenaltiesList(LeaseDTO lse, DateTime date, decimal? oldBal);

        public abstract string RuleName { get; }


        public List<BillPenalty> GetPenalties(LeaseDTO lse, DateTime date, decimal? oldBal)
        {
            ValidateRuleName(lse);
            return GetPenaltiesList(lse, date, oldBal);
        }


        private void ValidateRuleName(LeaseDTO lse)
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
