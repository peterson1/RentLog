using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public class DailyBiller1 : IDailyBiller
    {
        private RentBillComposer1     _rentComposr;
        private RightsBillComposer1   _rightsComposr;
        private ElectricBillComposer1 _electricComposr;
        private WaterBillComposer1    _waterComposr;


        public DailyBiller1(ICollectionsDir collectionsDir)
        {
            _rentComposr     = new RentBillComposer1(collectionsDir);
            _rightsComposr   = new RightsBillComposer1(collectionsDir);
            _electricComposr = new ElectricBillComposer1(collectionsDir);
            _waterComposr    = new WaterBillComposer1(collectionsDir);
        }


        public BillState ComputeBill(BillCode billCode, LeaseDTO lease, DateTime date, decimal? previousBalance)
        {
            var composr = GetRowComposer(billCode);
            var state   = new BillState
            {
                BillCode       = billCode,
                OpeningBalance = previousBalance,
                Penalties      = composr.ComputePenalties(lease, date, previousBalance),
                Payments       = composr.ReadPayments(lease, date),
                Adjustments    = composr.ReadAdjustments(lease, date),
            };
            state.ClosingBalance = composr.ComputeClosingBalance(lease, state, date);
            return state;
        }


        private IBillRowComposer GetRowComposer(BillCode billCode)
        {
            switch (billCode)
            {
                case BillCode.Rent    : return _rentComposr;
                case BillCode.Rights  : return _rightsComposr;
                case BillCode.Electric: return _electricComposr;
                case BillCode.Water   : return _waterComposr;
                default:
                    throw Fault.BadArg("BillCode", billCode);
            }
        }
    }
}
