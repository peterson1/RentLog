using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.SectionTabs.IntendedCollections
{
    [AddINotifyPropertyChangedInterface]
    public class IntendedColxnsVM : EncoderListVMBase<IntendedColxnDTO>
    {
        private SectionTabVM _tab;


        public IntendedColxnsVM(SectionTabVM sectionTabVM, SectionDTO sec, MainWindowVM main) 
            : base(main.ColxnsDB.IntendedColxns[sec.Id], main)
        {
            _tab       = sectionTabVM;
            CanAddRows = false;
            Caption    = $"{ListTitle} by ";
            FillCollectorsList(main.ColxnsDB.GetCollector(sec));

            CurrentCollectorChanged += (s, e) 
                => main.ColxnsDB.SetCollector(sec, e);
        }


        protected override void LoadRecordForEditing(IntendedColxnDTO rec) => _tab.EditIntendedColxn(rec);
        protected override bool CanDeleteRecord(IntendedColxnDTO rec) => Main.CanEncode;
        protected override string ListTitle => "Lease Collections";
        protected override Func<IntendedColxnDTO, LeaseDTO> LeaseGetter  => _ => _.Lease;
        protected override Func<IntendedColxnDTO, decimal>  SummedAmount => _ => _.Actuals.Total;
    }
}
