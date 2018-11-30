using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using RentLog.LeasesCrud.LeaseCRUD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.LeasesCrud.LeasesList
{
    [AddINotifyPropertyChangedInterface]
    public class InactiveLeasesVM : IndirectFilteredListVMBase<LeaseBalanceRow, InactiveLeaseDTO, LeasesFilterVM, AppArguments>
    {
        public InactiveLeasesVM(AppArguments appArguments) : base(appArguments.MarketState.InactiveLeases, appArguments, false)
        {
        }


        protected override void OnItemOpened(InactiveLeaseDTO e)
            => SoaViewer.Show(e, AppArgs);


        protected override List<InactiveLeaseDTO> QueryItems(ISimpleRepo<InactiveLeaseDTO> db)
        {
            var items = db.GetAll().OrderByDescending(_ => _.DeactivatedDate).ToList();
            Caption   = $"  Inactive Leases  ({items.Count:N0})  ";
            return items;
        }


        protected override LeaseBalanceRow CastToRow(InactiveLeaseDTO lse)
        {
            var repo = AppArgs.Balances.GetRepo(lse);
            DailyBillDTO bill = null;
            try
            {
                bill = repo.Latest();
            }
            catch (Exception ex)
            {
                var desc = $"[{lse.Id}] “{lse.TenantAndStall}”";
                Alert.Show(ex, $"Reading latest bill row for {desc}");
            }
            return new LeaseBalanceRow(lse, bill);
        }


        protected override string MainMethodCmdLabel => "Add Stall to this Tenant";
        protected override bool   CanRunMainMethod() => AppArgs.CanAddLease(false);
        protected override void   RunMainMethod()
        {
            if (!TryGetPickedItem(out InactiveLeaseDTO lse)) return;
            var crud = new LeaseCrudVM(AppArgs.MarketState.ActiveLeases, AppArgs);
            crud.TenantTemplate = lse.Tenant.ShallowClone();
            crud.DraftBirthDate = lse.Tenant.BirthDate;
            crud.EncodeNewDraftCmd.ExecuteIfItCan();
        }
    }
}
