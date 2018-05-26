using System;
using System.Collections.Generic;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;

namespace RentLog.Cashiering.CashierCollections
{
    [AddINotifyPropertyChangedInterface]
    public class CashierColxnsVM : EncoderListVMBase<CashierColxnDTO, AppArguments>
    {
        public CashierColxnsVM(ISimpleRepo<CashierColxnDTO> repository, AppArguments appArguments) : base(repository, appArguments)
        {
            Caption = "Tenant Payments to Office";
        }


        protected override IEnumerable<CashierColxnDTO> PostProcessQueried(IEnumerable<CashierColxnDTO> items)
        {
            foreach (var item in items)
                AppArgs.MarketState.RefreshStall(item.Lease);

            return items;
        }

        protected override Func<CashierColxnDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
