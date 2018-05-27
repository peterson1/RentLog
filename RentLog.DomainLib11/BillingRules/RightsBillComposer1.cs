using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.BillingRules.RightsPenalties;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public class RightsBillComposer1 : BillRowComposerBase
    {
        protected override BillCode BillCode => BillCode.Rights;


        public RightsBillComposer1(ICollectionsDir collectionsDir) : base(collectionsDir)
        {
        }


        public override List<BillPenalty> ComputePenalties(LeaseDTO lse, DateTime date, decimal? previousBalance)
        {
            switch (lse.Rights.PenaltyRule)
            {
                case RightsDailyAfter90Surcharger.RULE:
                    return new RightsDailyAfter90Surcharger()
                        .GetPenalties(lse, date, previousBalance);

                default:
                    throw Fault.BadArg(nameof(lse.Rent.PenaltyRule), lse.Rent.PenaltyRule);
            }
        }


        protected override decimal GetRegularDue(LeaseDTO lse, BillState billState, DateTime date)
            => lse.ContractStart == date.Date 
             ? lse.Rights.TotalAmount : 0;
    }
}
