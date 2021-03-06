﻿using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.Cashiering.SectionTabs.IntendedCollections
{
    [AddINotifyPropertyChangedInterface]
    public class IntendedColxnCrudVM : RepoCrudWindowVMBase<IIntendedColxnsRepo, IntendedColxnDTO, IntendedColxnCrudWindow, ITenantDBsDir>
    {
        private int _suggestedPRNum;

        public IntendedColxnCrudVM(int suggestedPRNumber, UncollectedLeaseDTO uncollectedLeaseDTO, IIntendedColxnsRepo repository, ITenantDBsDir appArguments) : base(repository, appArguments)
        {
            Intention       = uncollectedLeaseDTO;
            _suggestedPRNum = suggestedPRNumber;
        }


        public UncollectedLeaseDTO  Intention  { get; }
        public decimal              UITotal    { get; private set; }


        protected override void ModifyDraftForInserting(IntendedColxnDTO draft)
        {
            draft.PRNumber      = _suggestedPRNum;
            draft.Lease         = Intention.Lease;
            draft.StallSnapshot = Intention.StallSnapshot;
            draft.Targets       = Intention.Targets;
            draft.Actuals       = new BillAmounts();
            draft.Actuals.Rent  = Intention.Lease.Rent.RegularRate;
        }


        protected override bool IsValidDraft(IntendedColxnDTO draft, out string whyInvalid)
        {
            UITotal = draft.Actuals.Total;
            return base.IsValidDraft(draft, out whyInvalid);
        }


        public    override string TypeDescription => "Lease Collection";
        protected override string CaptionPrefix   => "Lease Collection";
    }
}
