using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.Authorization;
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


        protected override bool CanRunMainMethod()
        {
            if (!AppArgs.CanIssueChequeToPayee(false))
                return CantDo($"[{AppArgs.Credentials.Roles}] NOT authorized");

            if (!TryGetPickedItem(out ChequeVoucherDTO dto))
                return CantDo("No selected item");

            return true;
        }


        protected override void RunMainMethod()
        {
            var chq = ItemsList.CurrentItem;

            if (!PopUpInput.TryGetDate("Issued Date",
                out DateTime date, DateTime.Now.Date)) return;

            if (!PopUpInput.TryGetString("Issued To",
                out string issuedTo, chq.Request.Payee)) return;

            AppArgs.Vouchers.SetAs_Issued(chq, date, issuedTo);
        }


        protected override List<ChequeVoucherDTO> QueryItems(ISimpleRepo<ChequeVoucherDTO> db)
            => (db as IChequeVouchersRepo)?.GetNonIssuedCheques();


        protected override string MainMethodCmdLabel => "Issue Cheque to Payee";
        protected override Func<ChequeVoucherDTO, decimal> SummedAmount => _ => _.Request.Amount ?? 0;
    }
}
