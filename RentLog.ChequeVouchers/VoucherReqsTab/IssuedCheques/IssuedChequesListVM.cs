using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
using RentLog.ChequeVouchers.CommonControls.ChequeVoucherViewer;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.ChequeVouchers.VoucherReqsTab.IssuedCheques
{
    public class IssuedChequesListVM : FilteredSavedListVMBase<ChequeVoucherDTO, ChequeVouchersFilterVM, ITenantDBsDir>
    {
        public IssuedChequesListVM(ITenantDBsDir dir) 
            : base(dir.Vouchers.PreparedCheques, dir, false)
        {
            Caption           = "Issued Cheques";
            ViewVoucherCmd    = R2Command.Relay(_ => OnItemOpened(ItemsList.CurrentItem), null, "View Voucher Details");
            EditIssuanceCmd   = R2Command.Relay(EditIssuanceDetails, _ => AppArgs.CanIssueChequeToPayee(false), "Edit Issuance Details");
            TakeBackIssuedCmd = R2Command.Relay(TakeBackIssuedCheque, _ => CanTakeBackIssuedCheque(), "Take Back Issued Cheque");
        }


        public IR2Command  ViewVoucherCmd     { get; }
        public IR2Command  EditIssuanceCmd    { get; }
        public IR2Command  TakeBackIssuedCmd  { get; }


        private void EditIssuanceDetails()
        {
            var e = ItemsList.CurrentItem;

            if (!PopUpInput.TryGetDate("Issued Date",
                out DateTime date, e.IssuedDate)) return;

            if (!PopUpInput.TryGetString("Issued To",
                out string issuedTo, e.IssuedTo)) return;

            e.IssuedDate = date;
            e.IssuedTo   = issuedTo;
            AppArgs.Vouchers.PreparedCheques.Update(e);
        }


        protected override void OnItemOpened(ChequeVoucherDTO e)
            => ChequeVoucherViewerVM.Show(ItemsList.CurrentItem, AppArgs);


        protected override bool CanRunMainMethod()
        {
            if (!AppArgs.CanMarkChequeAsCleared(false))
                return CantDo($"[{AppArgs.Credentials.Roles}] NOT authorized");

            if (!TryGetPickedItem(out ChequeVoucherDTO dto))
                return CantDo("No selected item");

            return true;
        }


        protected override void RunMainMethod()
        {
            if (!PopUpInput.TryGetDate("Cleared Date",
                out DateTime date, DateTime.Now.Date)) return;

            AppArgs.Vouchers.SetAs_Cleared(ItemsList.CurrentItem, date);
        }


        private bool CanTakeBackIssuedCheque()
        {
            if (!AppArgs.CanTakeBackIssuedCheque(false)) return false;

            return TryGetPickedItem(out ChequeVoucherDTO dto);

            //bool CmdCantDo(string whyNot)
            //{
            //    var cmd = TakeBackIssuedCmd;
            //    cmd.CurrentLabel = cmd.OriginalLabel + $"  --  {whyNot}";
            //    return false;
            //}

            //if (!AppArgs.CanTakeBackIssuedCheque(false))
            //    return CmdCantDo($"[{AppArgs.Credentials.Roles}] NOT authorized");

            //if (!TryGetPickedItem(out ChequeVoucherDTO dto))
            //    return CmdCantDo("No selected item");

            //return true;
        }


        private void TakeBackIssuedCheque()
        {
            var chq = ItemsList.CurrentItem;

            Alert.Confirm($"Are you sure you want to take back this issued cheque from {chq.IssuedTo}?", () 
                => AppArgs.Vouchers.SetAs_TakenBack(chq));
        }


        protected override List<ChequeVoucherDTO> QueryItems(ISimpleRepo<ChequeVoucherDTO> db)
            => (db as IChequeVouchersRepo)?.GetIssuedCheques(AppArgs.CurrentBankAcct.Id);


        protected override string MainMethodCmdLabel => "Mark Cheque as “Cleared”";
        protected override Func<ChequeVoucherDTO, decimal> SummedAmount => _ => _.Request.Amount ?? 0;
    }
}
