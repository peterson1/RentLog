using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
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
        protected override string CaptionPrefix   => "Lease";


        public LeaseCrudVM(IActiveLeasesRepo repository, AppArguments appArguments) : base(repository, appArguments)
        {
        }


        //todo: copy this to DTO before update
        //todo: fill this from DTO on load
        public DateTime?  DraftBirthDate    { get; set; }
                                            
        public double?    ContractSpanDays  { get; private set; }
        public DateTime?  FirstRentDueDate  { get; private set; }
        public DateTime?  RightsDueDate     { get; private set; }




        protected override LeaseDTO GetNewDraft()
        {
            if (!StallPicker.TryPick(AppArgs
                .MarketState, out StallDTO stall)) return null;

            var start  = DateTime.Now.Date;
            WhyInvalid = "Please fill up all required fields.";

            return new LeaseDTO
            {
                ContractStart = start,
                ContractEnd   = start.AddYears(1),
                Tenant        = new TenantModel { Country = "Philippines" },
                Stall         = stall,
                Rent          = stall.DefaultRent.ShallowClone(),
                Rights        = stall.DefaultRights.ShallowClone(),
            };
        }


        protected override void SaveNewRecord(LeaseDTO draft)
        {
            draft.Tenant.BirthDate     = DraftBirthDate.Value;
            draft.ApplicationSubmitted = draft.ContractStart;
            base.SaveNewRecord(draft);
        }


        public void UpdateDerivatives()
        {
            ContractSpanDays = (Draft?.ContractEnd - Draft?.ContractStart)?.TotalDays;
            FirstRentDueDate = Draft?.FirstRentDueDate;
            RightsDueDate    = Draft?.RightsDueDate;
        }


        protected override bool IsValidDraft(LeaseDTO draft, out string whyInvalid)
        {
            //todo: validate lease draft
            whyInvalid = "";
            return true;
        }


        protected override bool CanEncodeNewDraft() => AppArgs.CanAddLease(false);
    }
}
