using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.ChequeVouchers.VoucherReqsTab.IssuedCheques
{
    public class IssuedChequesListVM : FilteredSavedListVMBase<ChequeVoucherDTO, ChequeVouchersFilterVM, ITenantDBsDir>
    {
        public IssuedChequesListVM(ITenantDBsDir dir) 
            : base(dir.Vouchers.IssuedCheques, dir, false)
        {
            Caption = "Issued Cheques";
        }


        protected override Func<ChequeVoucherDTO, decimal> SummedAmount => _ => _.Request.Amount ?? 0;
    }
}
