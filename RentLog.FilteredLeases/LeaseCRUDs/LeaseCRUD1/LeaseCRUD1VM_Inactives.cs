using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.StateTransitions;
using RentLog.DomainLib45;
using RentLog.FilteredLeases.FilteredLists;
using RentLog.LeasesCrud.LeaseCRUD;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
                    crud.NewRecordSaved += (s, newLse) => ForwardBalances(inactv, newLse, args);
                    crud.EncodeNewDraftCmd.ExecuteIfItCan();
                });
        }


        private static void ForwardBalances(LeaseDTO inactv, LeaseDTO newLse, ITenantDBsDir dir)
        {
            //todo: implement this

            var oldRepo  = dir.Balances.GetRepo(inactv);
            var oldBills = oldRepo?.Latest();
            var adjsRepo = dir.Collections.For(newLse.ContractStart).BalanceAdjs;
            foreach (var billCode in BillCodes.Collected())
            {
                var oldBal = oldBills.For(billCode);
                if ((oldBal.ClosingBalance ?? 0) == 0) continue;
                adjsRepo.Insert(new BalanceAdjustmentDTO
                {
                    AmountOffset = oldBal.ClosingBalance.Value,
                    BillCode     = billCode,
                    DocumentRef  = "-",
                    LeaseId      = newLse.Id,
                    Reason       = "Forwarded Balance",
                });
            }
        }
    }
}
