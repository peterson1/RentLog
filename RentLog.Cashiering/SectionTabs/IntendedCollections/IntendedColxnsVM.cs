using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.SectionTabs.IntendedCollections
{
    [AddINotifyPropertyChangedInterface]
    public class IntendedColxnsVM : EncoderListVMBase<IntendedColxnDTO>
    {
        protected override string ListTitle => "Lease Collections";


        public IntendedColxnsVM(CollectorDTO collector, SectionDTO sec, MainWindowVM main) 
            : base(main.ColxnsDB.IntendedColxns[sec.Id], main)
        {
            CanAddRows = false;
            Caption    = $"{ListTitle} by {collector}";
        }


        protected override Func<IntendedColxnDTO, LeaseDTO> LeaseGetter  => _ => _.Lease;
        protected override Func<IntendedColxnDTO, decimal>  SummedAmount => _ => _.Actuals.Total;

    }
}
