using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules.RentPenalties
{
    public class RentMonthlySurcharger : RentPenaltyRuleBase
    {
        public override string RuleName => RentPenalty.MonthlySurcharge;


        protected override List<BillPenalty> GetPenaltiesList(LeaseDTO lse, DateTime date, decimal? oldBal)
        {
            if ((oldBal ?? 0) <= 0) return null;
            if (!lse.IsActive(date)) return null;
            var billDay  = (int)Math.Round(lse.Rent.PenaltyRate2);
            if (date.Day != billDay + 1) return null;
            var rate = lse.Rent.PenaltyRate1;
            return new List<BillPenalty>
            {
                new BillPenalty
                {
                    Label       = RuleName,
                    Amount      = GetPenaltyAmount (oldBal.Value, rate),
                    Computation = GetComputation   (oldBal.Value, rate)
                }
            };
        }


        protected virtual decimal GetPenaltyAmount(decimal oldBal, decimal rate) 
            => Math.Round(oldBal * rate, 0);


        protected virtual string GetComputation(decimal oldBal, decimal rate)
            => "balance * penaltyRate" + L.f
             + $"{oldBal:N2} * {rate:N2}" + L.f
             + "(centavos rounded off)";
    }
}
