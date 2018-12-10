using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.FilteredLeases.FilteredLists;

namespace RentLog.FilteredLeases.LeaseCRUDs.LeaseCRUD1
{
    public partial class LeaseCRUD1VM
    {


        public static IR2Command GetRenewInactiveLeaseCmd(FilteredListVMBase listVM,
            string label = "Renew Contract for this Lease")
        {
            return LeaseCRUD1VM.NewCmd(listVM, label,
                listVM.AppArgs.CanAddLease(false), (lse, crud) =>
                {
                    var args   = crud.AppArgs;
                    var inactv = lse as InactiveLeaseDTO;
                    if (inactv == null) throw Bad.Data("Expected non-null ‹InactiveLeaseDTO›");

                    if (args.MarketState.IsOccupied(inactv.Stall))
                    {
                        Alert.Show($"Stall [{inactv.Stall}] is currently occupied.");
                        return;
                    }

                    crud.TenantTemplate = inactv.Tenant.ShallowClone();
                    crud.DraftBirthDate = inactv.Tenant.BirthDate;
                    crud.SetPickedStall(inactv.Stall);
                    crud.SetPickedStart(inactv.DeactivatedDate.AddDays(1).Date);
                    crud.SetRenewedFrom(inactv);
                    crud.SetProductToSell(inactv.ProductToSell);
                    crud.NewRecordSaved += (s, newLse) => ForwardBalances(inactv, newLse, args);
                    crud.EncodeNewDraftCmd.ExecuteIfItCan();
                });
        }


        private static void ForwardBalances(InactiveLeaseDTO inactv, LeaseDTO newLse, ITenantDBsDir dir)
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
