using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.BillingRules
{
    public class ElectricBillComposer1 : BillRowComposerBase
    {
        protected override BillCode BillCode => BillCode.Electric;


        public ElectricBillComposer1(ICollectionsDir collectionsDir) : base(collectionsDir)
        {
        }


        public override List<DailyBillDTO.BillPenalty> ComputePenalties(LeaseDTO lse, DateTime date, decimal? previousBalance)
        {
            return null;
        }


        protected override decimal GetRegularDue(LeaseDTO lse, DailyBillDTO.BillState billState, DateTime date)
        {
            return 0;
        }
    }
}
