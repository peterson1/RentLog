using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.DomainLib45;
using RentLog.FilteredLeases.FilteredLists;
using RentLog.LeasesCrud.LeaseCRUD;
using System;
using System.Windows;

namespace RentLog.FilteredLeases.LeaseCRUDs.LeaseCRUD1
{
    public class LeaseCRUD1VM
    {
        public static IR2Command GetEncodeNewDraftCmd(FilteredListVMBase listVM, 
            string label = "Encode new Lease Contract")
        {
            if (!listVM.AppArgs.CanAddLease(false)) return null;
            return LeaseCRUD1VM.NewCmd(listVM, label, true,
                (lse, crud) => crud.EncodeNewDraftCmd.ExecuteIfItCan());
        }


        public static IR2Command GetAddStallToTenantCmd(FilteredListVMBase listVM, 
            string label = "Add another Stall to this Tenant")
        {
            return LeaseCRUD1VM.NewCmd(listVM, label,
                listVM.AppArgs.CanAddLease(false), (lse, crud) =>
                {
                    if (lse == null) return;
                    crud.TenantTemplate = lse.Tenant.ShallowClone();
                    crud.DraftBirthDate = lse.Tenant.BirthDate;
                    crud.EncodeNewDraftCmd.ExecuteIfItCan();
                });
        }


        public static IR2Command GetEditThisLeaseCmd(FilteredListVMBase listVM,
            string label = "Edit this Lease")
        {
            return LeaseCRUD1VM.NewCmd(listVM, label,
                listVM.AppArgs.CanEditLease(false), (lse, crud) =>
                {
                    if (lse == null) return;
                    crud.AllFieldsEnabled = true;
                    crud.EditCurrentRecord(lse);
                });
        }


        public static IR2Command GetEditTenantInfoCmd(FilteredListVMBase listVM,
            string label = "Edit Tenant Info")
        {
            return LeaseCRUD1VM.NewCmd(listVM, label,
                listVM.AppArgs.CanEditTenantInfo(false), (lse, crud) =>
            {
                if (lse == null) return;
                crud.AllFieldsEnabled = false;
                crud.EditCurrentRecord(lse);
            });
        }


        public static IR2Command GetTerminateThisLeaseCmd(FilteredListVMBase listVM,
            string label = "Terminate this Lease")
        {
            return LeaseCRUD1VM.NewCmd(listVM, label,
                listVM.AppArgs.CanTerminateteLease(false), (lse, crud) =>
            {
                if (lse == null) return;

                var resp = MessageBox.Show($"Are you sure you want to terminate the lease for {L.f} {lse}?",
                    "   Confirm Termination", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resp != MessageBoxResult.Yes) return;

                if (!PopUpInput.TryGetDate($"When is the last billable day for {L.f}{lse}?",
                    out DateTime termDate, DateTime.Now.Date)) return;

                if (!PopUpInput.TryGetString("Why are you terminating this lease?",
                    out string reason)) return;

                listVM.AppArgs.MarketState.DeactivateLease(lse, reason, termDate);
            });
        }


        private static IR2Command NewCmd(FilteredListVMBase listVM, string buttonLabel, 
            bool canExecute, Action<LeaseDTO, LeaseCrudVM> action)
        {
            var dir     = listVM.AppArgs;
            var repo    = dir.MarketState.ActiveLeases;
            var oldCrud = new LeaseCrudVM(repo, dir as AppArguments);
            oldCrud.SaveCompleted += (s, e) => listVM.DoAfterSave.Invoke();
            return R2Command.Relay(() =>
            {
                var lse = listVM.Rows.CurrentItem?.DTO;
                action(lse, oldCrud);
            }, 
            _ => canExecute, buttonLabel);
        }
    }
}
