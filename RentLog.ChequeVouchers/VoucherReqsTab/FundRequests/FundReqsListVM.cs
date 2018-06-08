using System;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests
{
    public class FundReqsListVM : FilteredSavedListVMBase<FundRequestDTO, FundReqsFilterVM, ITenantDBsDir>
    {
        public FundReqsListVM(ITenantDBsDir dir) 
            : base(dir.Vouchers.ActiveRequests, dir, false)
        {
            Caption = "For Cheque Preparation";
        }


        protected override Func<FundRequestDTO, decimal> SummedAmount => _ => _.Amount ?? 0;
    }
}
