using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.SectionTabs.NoOperations
{
    [AddINotifyPropertyChangedInterface]
    public class NoOperationsVM : EncoderListVMBase<UncollectedLeaseDTO, AppArguments>
    {
        protected override string ListTitle => "Did Not Operate";


        public NoOperationsVM(ISimpleRepo<UncollectedLeaseDTO> repository, AppArguments appArguments) : base(repository, appArguments)
        {
        }


        protected override Func<UncollectedLeaseDTO, decimal> SummedAmount => _ => _.Targets.Total;
    }
}
