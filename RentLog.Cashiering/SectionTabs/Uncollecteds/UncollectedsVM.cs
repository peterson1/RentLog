using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    [AddINotifyPropertyChangedInterface]
    public class UncollectedsVM : EncoderListVMBase<UncollectedLeaseDTO>
    {
        protected override string ListTitle => "Uncollecteds";


        public UncollectedsVM(SectionDTO sec, MainWindowVM main) 
            : base(main.ColxnsDB.Uncollecteds[sec.Id], main)
        {
            CanAddRows    = false;
            TotalVisible = false;
        }


        protected override Func<UncollectedLeaseDTO, LeaseDTO> LeaseGetter  => _ => _.Lease;
        protected override Func<UncollectedLeaseDTO, decimal>  SummedAmount => _ => _.Targets.Total;

    }
}
