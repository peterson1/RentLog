using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public class DailyBiller1 : IDailyBiller
    {
        private RentBillComposer1 _rentBillr;


        public DailyBiller1(ICollectionsDir collectionsDir)
        {
            _rentBillr = new RentBillComposer1(collectionsDir);
        }


        public BillState ComputeBill(BillCode billCode, LeaseDTO lease, DateTime date, decimal? previousBalance)
        {
            var composr = GetRowComposer(billCode);
            var state = new BillState
            {
                BillCode       = billCode,
                OpeningBalance = previousBalance,
                Penalties      = composr.ComputePenalties(lease, date, previousBalance),
                Payments       = composr.ReadPayments(lease, date),
                Adjustments    = composr.ReadAdjustments(lease, date),
            };
            state.ClosingBalance = composr.ComputeClosingBalance(state);
            return state;
        }


        private IBillRowComposer GetRowComposer(BillCode billCode)
        {
            switch (billCode)
            {
                case BillCode.Rent: return _rentBillr;
                default:
                    throw Fault.BadArg("BillCode", billCode);
            }
        }
    }
}
