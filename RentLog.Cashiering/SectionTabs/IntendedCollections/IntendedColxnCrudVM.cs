﻿using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.Cashiering.SectionTabs.IntendedCollections
{
    public class IntendedColxnCrudVM : RepoCrudWindowVMBase<IIntendedColxnsRepo, IntendedColxnDTO, IntendedColxnCrudWindow, ITenantDBsDir>
    {
        public IntendedColxnCrudVM(UncollectedLeaseDTO uncollectedLeaseDTO, IIntendedColxnsRepo repository, ITenantDBsDir appArguments) : base(repository, appArguments)
        {
            Intention = uncollectedLeaseDTO;
        }


        public UncollectedLeaseDTO  Intention  { get; }


        protected override void ModifyDraftForInserting(IntendedColxnDTO draft)
        {
            draft.Lease   = Intention.Lease;
            draft.Targets = Intention.Targets;
            draft.Actuals = new BillAmounts();
        }


        public    override string TypeDescription => "Lease Collection";
        protected override string CaptionPrefix   => "Lease Collection";
    }
}
