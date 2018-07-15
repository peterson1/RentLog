using CommonTools.Lib45.BaseViewModels;
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
        public decimal              UITotal    { get; private set; }


        protected override void ModifyDraftForInserting(IntendedColxnDTO draft)
        {
            draft.Lease         = Intention.Lease;
            draft.StallSnapshot = Intention.StallSnapshot;
            draft.Targets       = Intention.Targets;
            draft.Actuals       = new BillAmounts();
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
