using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    [AddINotifyPropertyChangedInterface]
    public class UncollectedsVM : EncoderListVMBase<UncollectedLeaseDTO>
    {
        protected override string ListTitle => "Uncollecteds";


        public UncollectedsVM(ISimpleRepo<UncollectedLeaseDTO> repository, AppArguments appArguments) : base(repository, appArguments)
        {
            CanAddRows    = false;
            TotalVisible = false;
        }


        protected override Func<UncollectedLeaseDTO, LeaseDTO> LeaseGetter  => _ => _.Lease;
        protected override Func<UncollectedLeaseDTO, decimal>  SummedAmount => _ => _.Targets.Total;

    }
}
