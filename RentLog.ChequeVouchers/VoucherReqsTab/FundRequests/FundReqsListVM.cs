using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
using RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud;
using RentLog.DomainLib11.Authorization;
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
            Crud    = new FundRequestCrudVM(dir);
        }


        public FundRequestCrudVM  Crud  { get; }


        protected override List<FundRequestDTO> QueryItems(ISimpleRepo<FundRequestDTO> db)
            => db.Find(_ => _.BankAccountId == AppArgs.CurrentBankAcct.Id);


        protected override void OnItemOpened(FundRequestDTO e)
            => Crud.EditCurrentRecord(e);


        protected override bool CanRunMainMethod()
        {
            if (!AppArgs.CanInputChequeDetails(false))
                return CantDo($"[{AppArgs.Credentials.Roles}] NOT authorized");

            if (!TryGetPickedItem(out FundRequestDTO dto))
                return CantDo("No selected item");

            if (!dto.Amount.HasValue)
                return CantDo("Requested Amount is blank");

            return true;
        }


        protected override void RunMainMethod()
        {
            var req = ItemsList.CurrentItem;

            if (!PopUpInput.TryGetDate("Cheque Date", 
                out DateTime date, DateTime.Now.Date)) return;

            if (!PopUpInput.TryGetInt("Cheque Number",
                out int num)) return;

            AppArgs.Vouchers.SetAs_Prepared(req, date, num);
        }


        protected override string MainMethodCmdLabel => "Input Cheque Details";
        protected override Func<FundRequestDTO, decimal> SummedAmount => _ => _.Amount ?? 0;
    }
}
