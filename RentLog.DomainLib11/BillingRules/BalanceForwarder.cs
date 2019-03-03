using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.BillingRules
{
    public static class BalanceForwarder
    {
        public static void ForwardBalancesTo(this InactiveLeaseDTO inactv, LeaseDTO newLse, ITenantDBsDir dir)
        {
            var adjDate  = dir.Collections.UnclosedDate();
            var newAdjs  = dir.Collections.For(adjDate).BalanceAdjs;
            var oldAdjs = dir.Collections.For(inactv.DeactivatedDate).BalanceAdjs;
            var oldBills = dir.Balances.GetRepo(inactv).Latest();
            foreach (var billCode in BillCodes.Collected())
            {
                var oldBal = oldBills.For(billCode);
                if ((oldBal.ClosingBalance ?? 0) == 0) continue;
                newAdjs.Insert(new BalanceAdjustmentDTO
                {
                    AmountOffset = oldBal.ClosingBalance.Value,
                    BillCode     = billCode,
                    DocumentRef  = "-",
                    LeaseId      = newLse.Id,
                    Reason       = $"Forwarded Balance from [{inactv.Id}]",
                });

                oldAdjs.Insert(new BalanceAdjustmentDTO
                {
                    AmountOffset = oldBal.ClosingBalance.Value * -1,
                    BillCode     = billCode,
                    DocumentRef  = "-",
                    LeaseId      = inactv.Id,
                    Reason       = $"Forwarded Balance to [{newLse.Id}]",
                });
            }
        }
    }
}
