using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using RentLog.ChequeVouchers.CommonControls.ChequeVoucherViewer;
using RentLog.ChequeVouchers.VoucherReqsTab.ChequeVoucherPrints;
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
            Caption         = "Prepared Cheques";
            ViewVoucherCmd  = R2Command.Relay(_ => OnItemOpened(ItemsList.CurrentItem), null, "View Voucher Details");
            PrintVoucherCmd = R2Command.Relay(PrintVoucher, null, "Print Cheque Voucher");
            EditChequeCmd   = R2Command.Relay(EditChequeDetails, _ => AppArgs.CanInputChequeDetails(false), "Edit Cheque Details");
        }


        public IR2Command  ViewVoucherCmd   { get; }
        public IR2Command  PrintVoucherCmd  { get; }
        public IR2Command  EditChequeCmd    { get; }


        private void EditChequeDetails()
        {
            var e = ItemsList.CurrentItem;

            if (!PopUpInput.TryGetDate("Cheque Date",
                out DateTime date, e.ChequeDate)) return;

            if (!PopUpInput.TryGetInt("Cheque Number",
                out int num, e.ChequeNumber)) return;

            e.ChequeDate   = date;
            e.ChequeNumber = num;
            AppArgs.Vouchers.PreparedCheques.Update(e);
        }


        private void PrintVoucher()
            => ChequeVoucherPrint.Preview(ItemsList.CurrentItem, AppArgs);


        protected override void OnItemOpened(ChequeVoucherDTO e)
            => ChequeVoucherViewerVM.Show(ItemsList.CurrentItem, AppArgs);


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
            => (db as IChequeVouchersRepo)?.GetNonIssuedCheques(AppArgs.CurrentBankAcct.Id);


        protected override string MainMethodCmdLabel => "Issue Cheque to Payee";
        protected override Func<ChequeVoucherDTO, decimal> SummedAmount => _ => _.Request.Amount ?? 0;
    }
}
