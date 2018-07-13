using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using RentLog.DomainLib11.VoucherRules;
using System;
using System.Linq;

namespace RentLog.ChequeVouchers.CommonControls.ChequeVoucherViewer
{
    public class ChequeVoucherViewerVM : MainWindowVMBase<ITenantDBsDir>
    {
        public event EventHandler ClearedDateUpdated = delegate { };


        public ChequeVoucherViewerVM(ChequeVoucherDTO chequeVoucherDTO, ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            DTO                = chequeVoucherDTO;
            //ClearedDate        = clearedDate;
            EditClearedDateCmd = R2Command.Relay(EditClearedDate, _ => CanEditClearedDate(), "Edit Cleared Date");

            if (!DTO.Request.HasCashInBankEntry())
                DTO.Request.Allocations.AddCashInBankEntry(AppArgs.CurrentBankAcct, DTO.Request.Amount.Value);
        }


        public ChequeVoucherDTO  DTO                 { get; }
        public IR2Command        EditClearedDateCmd  { get; }
        public DateTime?         PickedDate          { get; set; }
        public PassbookRowDTO    PassbookRow         { get; set; }
        public IPassbookRowsRepo PassbookRepo        { get; set; }
        public DateTime?         ClearedDate         => PassbookRow?.TransactionDate;


        protected override string CaptionPrefix => "Cheque Voucher";


        public bool CanEditClearedDate() 
            => PassbookRow != null && PassbookRepo != null 
            && AppArgs.CanEditClearedDate(false);


        private void EditClearedDate()
        {
            var origDate = PassbookRow.TransactionDate;
            if (!PickedDate.HasValue)
            {
                if (!PopUpInput.TryGetDate("Cleared Date", out DateTime newDate, origDate)) return;
                PickedDate = newDate;
            }

            PassbookRepo.Delete(PassbookRow);
            PassbookRow.Id = 0;
            PassbookRow.DateOffset = PickedDate.Value.DaysSinceMin();
            PassbookRepo.Insert(PassbookRow);

            PassbookRepo.RecomputeBalancesFrom(origDate);
            ClearedDateUpdated?.Invoke(this, EventArgs.Empty);
            CloseWindow();
        }


        //private PassbookRowDTO FindPassbookRow(out IPassbookRowsRepo repo)
        //{
        //    var bankId = DTO.Request.BankAccountId;
        //    repo       = AppArgs.Passbooks.GetRepo(bankId);
        //    var rows   = repo.RowsFor(ClearedDate.Value);
        //    if (rows == null) return null;
        //    if (!rows.Any()) return null;
        //    var match = rows.SingleOrDefault(_ => _.DocRefId == DTO.Id);
        //    if (match == null) throw No.Match<ChequeVoucherDTO>("Id", DTO.Id);
        //    return match;
        //}


        public static ChequeVoucherViewerVM Show(ChequeVoucherDTO dto, ITenantDBsDir dir, bool launchWindow = true)
        {
            var vm = new ChequeVoucherViewerVM(dto, dir);
            if (launchWindow)
                vm.Show<ChequeVoucherViewerWindow>();
            return vm;
        }
    }
}
