using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.SectionTabs.AmbulantCollections
{
    [AddINotifyPropertyChangedInterface]
    public class AmbulantColxnsVM : EncoderListVMBase<AmbulantColxnDTO>
    {
        protected override string ListTitle => "Ambulant Collections";


        public AmbulantColxnsVM(ISimpleRepo<AmbulantColxnDTO> repository, AppArguments appArguments) : base(repository, appArguments)
        {
            TotalVisible = false;
        }


        protected override Func<AmbulantColxnDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
