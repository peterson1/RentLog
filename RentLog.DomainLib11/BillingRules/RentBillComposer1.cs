using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public class RentBillComposer1 : BillRowComposerBase
    {
        protected override BillCode BillCode => BillCode.Rent;


        public RentBillComposer1(ICollectionsDir collectionsDir) : base(collectionsDir)
        {
        }


        public override List<BillPenalty> ComputePenalties(LeaseDTO lse, DateTime date, decimal? previousBalance)
        {
            switch (lse.Rent.PenaltyRule)
            {
                case RentPenalty.DailySurcharge:
                    return new RentDailySurcharger()
                        .GetPenalties(lse, date, previousBalance);

                case RentPenalty.DailySurcharge_NoRoundOff:
                    return new RentDailySurchargerNoRoundOff()
                        .GetPenalties(lse, date, previousBalance);

                case RentPenalty.MonthlySurcharge:
                    return new RentMonthlySurcharger()
                        .GetPenalties(lse, date, previousBalance);

                default:
                    throw Fault.BadArg(nameof(lse.Rent.PenaltyRule), lse.Rent.PenaltyRule);
            }
        }


        protected override decimal GetRegularDue(LeaseDTO lse, DateTime date)
        {
            if (!lse.IsActive(date)) return 0;
            if (date.Date < lse.FirstRentDueDate.Date) return 0;
            switch (lse.Rent.Interval)
            {
                case BillInterval.Daily  : return GetDailyDue   (lse, date);
                case BillInterval.Monthly: return GetMonthlyDue (lse, date);
                default: throw Bad.Arg("Rent.Interval", lse.Rent.Interval);
            }
        }


        private static decimal GetDailyDue(LeaseDTO lse, DateTime date) 
            => lse.Rent.RegularRate;


        private decimal GetMonthlyDue(LeaseDTO lse, DateTime date)
            => date.Day == lse.Rent.PenaltyRate2
             ? lse.Rent.RegularRate : 0;
    }
}
