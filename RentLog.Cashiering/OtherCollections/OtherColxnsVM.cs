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
        public OtherColxnsVM(ISimpleRepo<OtherColxnDTO> repository, AppArguments appArguments) : base(repository, appArguments)
        {
            Caption = "Other Collections";
        }

        protected override Func<OtherColxnDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
