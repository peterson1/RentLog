using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.ChequeVouchers.VoucherReqsTab.PreparedCheques
{
    public class PreparedChequesListVM : FilteredSavedListVMBase<ChequeVoucherDTO, ChequeVouchersFilterVM, ITenantDBsDir>
    {
        public PreparedChequesListVM(ITenantDBsDir dir) 
            : base(dir.Vouchers.PreparedCheques, dir, false)
        {
            Caption = "Prepared Cheques";
        }


        protected override List<ChequeVoucherDTO> QueryItems(ISimpleRepo<ChequeVoucherDTO> db)
            => (db as IChequeVouchersRepo)?.GetNonIssuedCheques();


        protected override Func<ChequeVoucherDTO, decimal> SummedAmount => _ => _.Request.Amount ?? 0;
    }
}
