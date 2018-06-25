using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.ChequeVouchers.JournalsTab.JournalsCrud.AllocationsList;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.JournalVoucherRepos;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsCrud
{
    [AddINotifyPropertyChangedInterface]
    public class JournalsCrudVM : RepoCrudWindowVMBase<IJournalVouchersRepo, JournalVoucherDTO, JournalsCrudWindow, ITenantDBsDir>
    {
        public JournalsCrudVM(ITenantDBsDir dir) : base(dir.Journals, dir)
        {
        }


        public AllocationsListVM  Allocations      { get; } = new AllocationsListVM();
        public DateTime           TransactionDate  { get; set; }


        protected override async Task ModifyDraftForInsertAsync(JournalVoucherDTO draft)
        {
            TransactionDate   = DateTime.Now;
            draft.Allocations = new List<AccountAllocation>();
            Allocations.SetHost(draft.Allocations, AppArgs.MarketState.GLAccounts);
            draft.SerialNum = await AppArgs.Journals.GetNextSerialNum();
        }


        protected override void ModifyDraftForUpdating(JournalVoucherDTO draft)
            => Allocations.SetHost(draft.Allocations, AppArgs.MarketState.GLAccounts);


        public void OnTransactionDateChanged()
        {
            if (Draft != null)
                Draft.DateOffset = TransactionDate.DaysSinceMin();
        }


        public override bool   CanEncodeNewDraft() => AppArgs.CanAddJournalVoucher(false);
        public    override string TypeDescription     => "Journal Voucher";
        protected override string CaptionPrefix       => "Journal Voucher";
    }
}
