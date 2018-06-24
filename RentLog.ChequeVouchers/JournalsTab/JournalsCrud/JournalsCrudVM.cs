using CommonTools.Lib45.BaseViewModels;
using RentLog.ChequeVouchers.JournalsTab.JournalsCrud.AllocationsList;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.JournalVoucherRepos;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsCrud
{
    public class JournalsCrudVM : RepoCrudWindowVMBase<IJournalVouchersRepo, JournalVoucherDTO, JournalsCrudWindow, ITenantDBsDir>
    {
        public JournalsCrudVM(ITenantDBsDir dir) : base(dir.Journals, dir)
        {
        }


        public AllocationsListVM  Allocations  { get; } = new AllocationsListVM();


        protected override async void ModifyDraftForInserting(JournalVoucherDTO draft)
        {
            StartBeingBusy("Getting next serial number ...");
            await Task.Delay(1000 * 10);
            draft.SerialNum   = await AppArgs.Journals.GetNextSerialNum();
            draft.Allocations = new List<AccountAllocation>();
            Allocations.SetHost(draft.Allocations, AppArgs.MarketState.GLAccounts);
        }

        protected override void ModifyDraftForUpdating(JournalVoucherDTO draft)
            => Allocations.SetHost(draft.Allocations, AppArgs.MarketState.GLAccounts);


        public    override bool   CanEncodeNewDraft() => AppArgs.CanAddJournalVoucher(false);
        public    override string TypeDescription     => "Journal Voucher";
        protected override string CaptionPrefix       => "Journal Voucher";
    }
}
