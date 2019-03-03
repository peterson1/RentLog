using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DTOs;
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
                    crud.NewRecordSaved += (s, newLse) => inactv.ForwardBalancesTo(newLse, args);
                    crud.EncodeNewDraftCmd.ExecuteIfItCan();
                });
        }
    }
}
