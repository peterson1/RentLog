using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules.RentPenalties
{
    public class RentDailySurcharger : IBillSurcharger
    {
        public const string RULE = "Daily Surcharge";


        public List<BillPenalty> GetPenalties(LeaseDTO lse, DateTime date, decimal? oldBal)
        {
            ValidatePenaltyRule(lse);
            if ((oldBal ?? 0) <= 0) return null;
            if (!lse.IsActive(date)) return null;
            var rate = lse.Rent.PenaltyRate1;
            return new List<BillPenalty>
            {
                new BillPenalty
                {
                    Label       = RULE,
                    Amount      = Math.Round(oldBal.Value * rate, 0),
                    Computation = "balance * penaltyRate" + L.f
                                + $"{oldBal:N2} * {rate:N2}" + L.f 
                                + "(centavos rounded off)"
                }
            };
        }


        private void ValidatePenaltyRule(LeaseDTO lse)
        {
            if (lse?.Rent == null)
                throw Fault.NullRef("Lease Rent Params");

            if (lse.Rent.PenaltyRule != RULE)
                throw Fault.BadKey<LeaseDTO>(RULE, 
                    lse.Rent.PenaltyRule, 
                    nameof(lse.Rent.PenaltyRule));
        }
    }
}
