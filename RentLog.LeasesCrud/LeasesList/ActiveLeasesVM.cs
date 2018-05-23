using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;
using RentLog.DomainLib45.SoaViewer.MainWindow;
using RentLog.LeasesCrud.LeaseCRUD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.LeasesCrud.LeasesList
{
    [AddINotifyPropertyChangedInterface]
    public class ActiveLeasesVM : IndirectFilteredListVMBase<LeaseBalanceRow, LeaseDTO, LeasesFilterVM, AppArguments>
    {
        public ActiveLeasesVM(AppArguments appArguments) : base(appArguments.MarketState.ActiveLeases, appArguments, false)
        {
            Crud                  = new LeaseCrudVM(AppArgs.MarketState.ActiveLeases, AppArgs);
            AddStallToTenantCmd   = R2Command.Relay(AddStallToTenant  , _ => Crud.CanEncodeNewDraft(), "Add Stall to this Tenant");
            EditThisLeaseCmd      = R2Command.Relay(EditThisLease     , _ => CanEditRecord(Rows.CurrentItem?.DTO), "Edit this Lease");
            TerminateThisLeaseCmd = R2Command.Relay(TerminateThisLease, _ => AppArgs.CanTerminateteLease(false), "Terminate this Lease");
        }


        public string       Caption                { get; private set; }
        public LeaseCrudVM  Crud                   { get; }
        public IR2Command   AddStallToTenantCmd    { get; }
        public IR2Command   EditThisLeaseCmd       { get; }
        public IR2Command   TerminateThisLeaseCmd  { get; }


        private void TerminateThisLease()
        {
            Alert.Show($"Terminating {Rows.CurrentItem.DTO.TenantAndStall}");
        }


        private void EditThisLease()
        {
            Alert.Show($"Editing {Rows.CurrentItem.DTO.TenantAndStall}");
        }


        private void AddStallToTenant()
        {
            var lse = Rows.CurrentItem?.DTO;
            if (lse == null) return;
            Crud.TenantTemplate = lse.Tenant.ShallowClone();
            Crud.DraftBirthDate = lse.Tenant.BirthDate;
            Crud.EncodeNewDraftCmd.ExecuteIfItCan();
        }


        protected override void OnItemOpened(LeaseDTO e)
            => SoaViewer.Show(e, AppArgs);


        public override bool CanEditRecord(LeaseDTO rec) 
            => AppArgs.CanEditLease(false);


        protected override List<LeaseDTO> QueryItems(ISimpleRepo<LeaseDTO> db)
        {
            var items = db.GetAll().OrderByDescending(_ => _.Id).ToList();
            Caption   = $"  Active Leases  ({items.Count:N0})  ";
            return items;
        }


        protected override LeaseBalanceRow CastToRow(LeaseDTO lse)
        {
            var date = AppArgs.Collections.LastPostedDate();
            var bill = AppArgs.Balances.GetBill(lse, date);
            return new LeaseBalanceRow(lse, bill);
        }
    }
}
