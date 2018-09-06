using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
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
        private VoucherReqsTabVM _main;


        public PreparedChequesListVM(ITenantDBsDir dir, VoucherReqsTabVM vouchersTab) 
            : base(dir.Vouchers.PreparedCheques, dir, false)
        {
            _main               = vouchersTab;
            Caption             = "Prepared Cheques";
            ViewVoucherCmd      = R2Command.Relay(_ => OnItemOpened(ItemsList.CurrentItem), null, "View Voucher Details");
            PrintVoucherCmd     = R2Command.Relay(PrintVoucher, null, "Print Cheque Voucher");
            EditChequeCmd       = R2Command.Relay(EditChequeDetails, _ => AppArgs.CanInputChequeDetails(false), "Edit Cheque Details");
            RemoveChequeInfoCmd = R2Command.Relay(RemoveChequeInfo, _ => AppArgs.CanInputChequeDetails(false), "Remove Cheque Details");
            MarkAsCancelledCmd  = R2Command.Relay(MarkAsCancelled, _ => AppArgs.CanMarkChequeAsCancelled(false), "Mark Cheque as “Cancelled”");
        }


        public IR2Command  ViewVoucherCmd      { get; }
        public IR2Command  PrintVoucherCmd     { get; }
        public IR2Command  EditChequeCmd       { get; }
        public IR2Command  RemoveChequeInfoCmd { get; }
        public IR2Command  MarkAsCancelledCmd  { get; }


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


        private void RemoveChequeInfo()
        {
            Alert.Confirm("Are you sure you want to remove the details of the cheque?" 
                  + L.F + "Doing so will move this voucher" 
                  + L.f + " back to “For Check Preparation”." 
                  + L.F + "Do you want to proceed?", () =>
            {
                AppArgs.Vouchers.SetAs_Unprepared(ItemsList.CurrentItem);
                _main.ReloadAll();
            });
        }


        private void PrintVoucher()
            => ChequeVoucherPrint.Preview(ItemsList.CurrentItem, AppArgs);


        protected override void OnItemOpened(ChequeVoucherDTO e)
            => ChequeVoucherViewerVM.Show(ItemsList.CurrentItem, AppArgs);


        protected override bool CanDeleteRecord(ChequeVoucherDTO rec)
            => AppArgs.CanDeletePreparedCheque(true);


        protected override bool CanRunMainMethod()
        {
            if (!AppArgs.CanIssueChequeToPayee(false))
                return CantDo($"[{AppArgs.Credentials.Roles}] NOT authorized");

            if (!TryGetPickedItem(out ChequeVoucherDTO dto))
                return CantDo("No selected item");

            return true;
        }


        private void MarkAsCancelled()
        {
            Alert.Confirm("Mark cheque as “Cancelled”?", () =>
            {
                AppArgs.Vouchers.SetAs_Cancelled(ItemsList.CurrentItem);
                ReloadFromDB();
            });
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
