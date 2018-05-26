using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.SectionTabs.IntendedCollections
{
    [AddINotifyPropertyChangedInterface]
    public class IntendedColxnsVM : EncoderListVMBase<IntendedColxnDTO, AppArguments>
    {
        protected override string ListTitle => "Lease Collections";


        public IntendedColxnsVM(ISimpleRepo<IntendedColxnDTO> repository, AppArguments appArguments) : base(repository, appArguments)
        {
        }


        protected override Func<IntendedColxnDTO, decimal> SummedAmount => _ => _.Actuals.Total;

    }
}
