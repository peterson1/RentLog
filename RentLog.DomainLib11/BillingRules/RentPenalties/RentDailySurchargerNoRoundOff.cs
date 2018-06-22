using CommonTools.Lib11.StringTools;
using System;

namespace RentLog.DomainLib11.BillingRules.RentPenalties
{
    public class RentDailySurchargerNoRoundOff : RentDailySurcharger
    {
        public override string RuleName => RentPenalty.DailySurcharge_NoRoundOff;


        protected override decimal GetPenaltyAmount(decimal oldBal, decimal rate)
            => oldBal * rate;


        protected override string GetComputation(decimal oldBal, decimal rate)
            => "balance * penaltyRate" + L.f
             + $"{oldBal:N2} * {rate:N2}";
    }
}
