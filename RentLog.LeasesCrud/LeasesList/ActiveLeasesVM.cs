﻿using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib11.StateTransitions;
using RentLog.DomainLib45;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using RentLog.LeasesCrud.LeaseCRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RentLog.LeasesCrud.LeasesList
{
    [AddINotifyPropertyChangedInterface]
    public class ActiveLeasesVM : IndirectFilteredListVMBase<LeaseBalanceRow, LeaseDTO, LeasesFilterVM, AppArguments>
    {
        public event EventHandler LeaseDeactivated = delegate { };
        private DateTime _postdDate;


        public ActiveLeasesVM(AppArguments appArguments) : base(appArguments.MarketState.ActiveLeases, appArguments, false)
        {
            Crud                  = new LeaseCrudVM(AppArgs.MarketState.ActiveLeases, AppArgs);
            AddStallToTenantCmd   = R2Command.Relay(AddStallToTenant  , _ => Crud.CanEncodeNewDraft(), "Add another Stall to this Tenant");
            EditThisLeaseCmd      = R2Command.Relay(EditThisLease     , _ => CanEditRecord(Rows.CurrentItem?.DTO), "Edit this Lease");
            EditTenantInfoCmd     = R2Command.Relay(EditThisLease     , _ => AppArgs.CanEditTenantInfo(false), "Edit Tenant Info");
            TerminateThisLeaseCmd = R2Command.Relay(TerminateThisLease, _ => AppArgs.CanTerminateteLease(false), "Terminate this Lease");
        }


        public LeaseCrudVM  Crud                   { get; }
        public IR2Command   AddStallToTenantCmd    { get; }
        public IR2Command   EditThisLeaseCmd       { get; }
        public IR2Command   EditTenantInfoCmd      { get; }
        public IR2Command   TerminateThisLeaseCmd  { get; }


        private void AddStallToTenant()
        {
            if (!TryGetPickedItem(out LeaseDTO lse)) return;
            Crud.TenantTemplate = lse.Tenant.ShallowClone();
            Crud.DraftBirthDate = lse.Tenant.BirthDate;
            Crud.EncodeNewDraftCmd.ExecuteIfItCan();
        }


        private void EditThisLease()
        {
            if (!TryGetPickedItem(out LeaseDTO lse)) return;
            Crud.AllFieldsEnabled = AppArgs.CanEditLease(false);
            Crud.EditCurrentRecord(lse);
        }


        private void TerminateThisLease()
        {
            if (!TryGetPickedItem(out LeaseDTO lse)) return;

            var resp = MessageBox.Show($"Are you sure you want to terminate the lease for {L.f} {lse}?",
                "   Confirm Termination", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resp != MessageBoxResult.Yes) return;

            if (!PopUpInput.TryGetDate($"When is the last billable day for {L.f}{lse}?",
                out DateTime termDate, DateTime.Now.Date)) return;

            AppArgs.MarketState.DeactivateLease(lse, "Manual Termination", termDate);
            LeaseDeactivated?.Invoke(this, EventArgs.Empty);
        }


        protected override void OnItemOpened(LeaseDTO e)
            => SoaViewer.Show(e, AppArgs);


        public override bool CanEditRecord(LeaseDTO rec) 
            => AppArgs.CanEditLease(false);


        protected override List<LeaseDTO> QueryItems(ISimpleRepo<LeaseDTO> db)
        {
            _postdDate = AppArgs.Collections.LastPostedDate();
            var items  = db.GetAll().OrderByDescending(_ => _.Id).ToList();
            Caption    = $"  Active Leases  ({items.Count:N0})  ";
            return items;
        }


        protected override LeaseBalanceRow CastToRow(LeaseDTO lse)
        {
            DailyBillDTO bill = null;
            try
            {
                bill = AppArgs.Balances.GetBill(lse, _postdDate);
            }
            catch (Exception ex)
            {
                Alert.Show(ex, $"Getting bill for [{lse.Id}] “{lse}”");
            }
            return new LeaseBalanceRow(lse, bill);
        }
    }
}
