﻿using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.CashierCollections
{
    [AddINotifyPropertyChangedInterface]
    public class CashierColxnsVM : EncoderListVMBase<CashierColxnDTO>
    {
        protected override string ListTitle => "Tenant Payments to Office";


        public CashierColxnsVM(MainWindowVM main) : base(main.ColxnsDB.CashierColxns, main)
        {
        }


        //protected override IEnumerable<CashierColxnDTO> PostProcessQueried(IEnumerable<CashierColxnDTO> items)
        //{
        //    foreach (var item in items)
        //        AppArgs.MarketState.RefreshStall(item.Lease);
        //
        //    return items;
        //}


        protected override Func<CashierColxnDTO, LeaseDTO> LeaseGetter => _ => _.Lease;
        protected override Func<CashierColxnDTO, decimal>  SummedAmount => _ => _.Amount;
    }
}
