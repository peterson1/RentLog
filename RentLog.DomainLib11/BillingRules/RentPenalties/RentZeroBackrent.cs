using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.BillingRules.RentPenalties
{
    public class RentZeroBackrent : RentPenaltyRuleBase
    {
        public override string RuleName => RentPenalty.ZeroBackrent;


        protected override List<DailyBillDTO.BillPenalty> GetPenaltiesList(LeaseDTO lse, DateTime date, decimal? oldBal)
            => null;
    }
}
