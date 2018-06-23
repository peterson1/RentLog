using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules.RightsPenalties
{
    public class RightsDailyAfter90Surcharger : RightsPenaltyRuleBase
    {
        public override string RuleName => RightsPenalty.DailyAfter90Days;


        protected override List<BillPenalty> GetPenaltiesList(LeaseDTO lse, DateTime date, decimal? oldBal)
        {
            if ((oldBal ?? 0) <= 0) return null;
            if (!lse.IsActive(date)) return null;

            if (IsDayAfterDue(date, lse))
                return Penalty(lse.Rights.PenaltyRate1, oldBal);

            else if (IsBeyond90Days(date, lse))
                return Penalty(lse.Rights.PenaltyRate2, oldBal);
            else
                return null;
        }


        private bool IsDayAfterDue(DateTime date, LeaseDTO lse)
            => date.Date == lse.RightsDueDate.Date.AddDays(1);


        private bool IsBeyond90Days(DateTime date, LeaseDTO lse)
            => date.Date >= lse.RightsDueDate.Date.AddDays(90);


        private List<BillPenalty> Penalty(decimal rate, decimal? oldBal) => new List<BillPenalty> { new BillPenalty
        {
            Label       = RuleName,
            Amount      = GetPenaltyAmount (oldBal.Value, rate),
            Computation = GetComputation   (oldBal.Value, rate)
        }};


        protected virtual decimal GetPenaltyAmount(decimal oldBal, decimal rate) 
            => Math.Round(oldBal * rate, 0);


        protected virtual string GetComputation(decimal oldBal, decimal rate)
            => "balance * penaltyRate" + L.f
             + $"{oldBal:N2} * {rate:N2}" + L.f
             + "(centavos rounded off)";
    }
}
