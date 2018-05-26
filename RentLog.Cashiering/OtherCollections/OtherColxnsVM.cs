using System;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;

namespace RentLog.Cashiering.OtherCollections
{
    [AddINotifyPropertyChangedInterface]
    public class OtherColxnsVM : EncoderListVMBase<OtherColxnDTO, AppArguments>
    {
        public OtherColxnsVM(ISimpleRepo<OtherColxnDTO> repository, AppArguments appArguments) : base(repository, appArguments, false)
        {
            Caption = "Other Collections";
        }

        protected override Func<OtherColxnDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
