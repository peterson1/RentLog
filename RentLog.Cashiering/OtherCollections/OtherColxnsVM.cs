using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.OtherCollections
{
    [AddINotifyPropertyChangedInterface]
    public class OtherColxnsVM : EncoderListVMBase<OtherColxnDTO, AppArguments>
    {
        protected override string ListTitle => "Other Collections";


        public OtherColxnsVM(ISimpleRepo<OtherColxnDTO> repository, AppArguments appArguments) : base(repository, appArguments)
        {
        }


        protected override Func<OtherColxnDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
