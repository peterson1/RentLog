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


        public ChequeVoucherViewerVM(ChequeVoucherDTO chequeVoucherDTO, DateTime? clearedDate, ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            DTO                = chequeVoucherDTO;
            ClearedDate        = clearedDate;
            EditClearedDateCmd = R2Command.Relay(EditClearedDate, _ => CanEditClearedDate(), "Edit Cleared Date");

            if (!DTO.Request.HasCashInBankEntry())
                DTO.Request.Allocations.AddCashInBankEntry(AppArgs.CurrentBankAcct, DTO.Request.Amount.Value);
        }


        public ChequeVoucherDTO  DTO                 { get; }
        public DateTime?         ClearedDate         { get; }
        public IR2Command        EditClearedDateCmd  { get; }


        protected override string CaptionPrefix => "Cheque Voucher";


        private bool CanEditClearedDate() 
            => ClearedDate.HasValue && AppArgs.CanEditClearedDate(true);


        private void EditClearedDate()
        {
            if (!PopUpInput.TryGetDate("Cleared Date", out DateTime newDate, ClearedDate)) return;
            var match = FindPassbookRow(out IPassbookRowsRepo repo);
            if (match == null) throw No.Match<PassbookRowDTO>("ClearedDate", ClearedDate);
            match.DateOffset = newDate.DaysSinceMin();
            repo.Delete(match);
            match.Id = 0;
            repo.Insert(match);//todo: test this
            //repo.Update(match);
            repo.RecomputeBalancesFrom(ClearedDate.Value);
            ClearedDateUpdated?.Invoke(this, EventArgs.Empty);
            CloseWindow();
        }


        private PassbookRowDTO FindPassbookRow(out IPassbookRowsRepo repo)
        {
            var bankId = DTO.Request.BankAccountId;
            repo       = AppArgs.Passbooks.GetRepo(bankId);
            var rows   = repo.RowsFor(ClearedDate.Value);
            if (rows == null) return null;
            if (!rows.Any()) return null;
            var match = rows.SingleOrDefault(_ => _.DocRefId == DTO.Id);
            if (match == null) throw No.Match<ChequeVoucherDTO>("Id", DTO.Id);
            return match;
        }


        public static ChequeVoucherViewerVM Show(ChequeVoucherDTO dto, DateTime? clearedDate, ITenantDBsDir dir)
        {
            var vm = new ChequeVoucherViewerVM(dto, clearedDate, dir);
            vm.Show<ChequeVoucherViewerWindow>();
            return vm;
        }
    }
}
