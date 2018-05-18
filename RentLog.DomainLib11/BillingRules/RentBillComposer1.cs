using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
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
                case RentDailySurcharger.RULE:
                    return new RentDailySurcharger()
                        .GetPenalties(lse, date, previousBalance);

                default:
                    throw Fault.BadArg(nameof(lse.Rent.PenaltyRule), lse.Rent.PenaltyRule);
            }
        }


        public override decimal TotalDue(BillState billState)
        {
            throw new NotImplementedException();
        }
    }
}
