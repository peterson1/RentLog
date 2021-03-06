﻿using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45;
using RentLog.DomainLib45.StallPicker;
using System;

namespace RentLog.LeasesCrud.LeaseCRUD
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseCrudVM : RepoCrudWindowVMBase<IActiveLeasesRepo, LeaseDTO, LeaseCrudWindow, AppArguments>
    {
        public    override string TypeDescription => "Lease";
        protected override string CaptionPrefix   => "Lease Editor";
        private StallDTO  _pickedStall;
        private DateTime? _pickedStart;
        private int?      _renewedFromID;
        private string    _productToSell;


        public LeaseCrudVM(IActiveLeasesRepo repository, AppArguments appArguments) : base(repository, appArguments)
        {
        }


        public DateTime?    DraftBirthDate    { get; set; }
        public TenantModel  TenantTemplate    { get; set; }
        public bool         AllFieldsEnabled  { get; set; }

        public double?      ContractSpanDays  { get; private set; }
        public DateTime?    FirstRentDueDate  { get; private set; }
        public DateTime?    RightsDueDate     { get; private set; }

        public UIList<string> RentPenaltyRules { get; } = new UIList<string>(RentPenalty.Rules);



        protected override LeaseDTO GetNewDraft()
        {
            if (_pickedStall == null)
            {
                if (!StallPicker.TryPick(AppArgs
                    .MarketState, out _pickedStall)) return null;
            }

            var start  = _pickedStart ?? DateTime.Now.Date;
            AllFieldsEnabled = true;
            WhyInvalid = "Please fill up all required fields.";

            var draft = new LeaseDTO
            {
                ContractStart        = start,
                ContractEnd          = start.AddYears(1).Date,
                Stall                = _pickedStall,
                Rent                 = _pickedStall.DefaultRent.ShallowClone(),
                Rights               = _pickedStall.DefaultRights.ShallowClone(),
                RenewedFromID        = _renewedFromID,
                ProductToSell        = _productToSell,
            };
            draft.Tenant = TenantTemplate?.ShallowClone()
                         ?? new TenantModel { Country = "Philippines" };
            return draft;
        }


        public void OnDraftBirthDateChanged()
        {
            if (Draft?.Tenant == null) return;
            Draft.Tenant.BirthDate = DraftBirthDate ?? DateTime.MinValue;
        }


        protected override LeaseDTO CreateDraftFromRecord(LeaseDTO record)
        {
            DraftBirthDate = record.Tenant.BirthDate;
            return base.CreateDraftFromRecord(record);
        }


        protected override void ClearDraftAfterSave()
        {
            DraftBirthDate = null;
            TenantTemplate = null;
            _pickedStall   = null;
            base.ClearDraftAfterSave();
        }


        public void SetPickedStall  (StallDTO stallDTO   ) => _pickedStall   = stallDTO;
        public void SetPickedStart  (DateTime startDate  ) => _pickedStart   = startDate;
        public void SetRenewedFrom  (LeaseDTO lease      ) => _renewedFromID = lease.Id;
        public void SetProductToSell(string productToSell) => _productToSell = productToSell;


        public void UpdateDerivatives()
        {
            ContractSpanDays = (Draft?.ContractEnd - Draft?.ContractStart)?.TotalDays;
            FirstRentDueDate = Draft?.FirstRentDueDate;
            RightsDueDate    = Draft?.RightsDueDate;
        }


        //protected override bool IsValidDraft(LeaseDTO draft, out string whyInvalid)
        //{
        //    if (!base.IsValidDraft(draft, out whyInvalid)) return false;
        //    if (draft.ContractStart >= draft.ContractEnd)
        //    {
        //        whyInvalid = "“Contract End” date should be later than “Contract Start” date.";
        //        return false;
        //    }
        //    whyInvalid = "";
        //    return true;
        //}


        public override bool CanEncodeNewDraft() => AppArgs.CanAddLease(false);
    }
}
