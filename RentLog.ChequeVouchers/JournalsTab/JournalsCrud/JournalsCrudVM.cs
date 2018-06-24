using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.JournalVoucherRepos;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsCrud
{
    public class JournalsCrudVM : RepoCrudWindowVMBase<IJournalVouchersRepo, JournalVoucherDTO, JournalsCrudWindow, ITenantDBsDir>
    {
        public JournalsCrudVM(ITenantDBsDir dir) : base(dir.Journals, dir)
        {
        }


        public    override bool   CanEncodeNewDraft() => AppArgs.CanAddJournalVoucher(false);
        public    override string TypeDescription     => "Journal Voucher";
        protected override string CaptionPrefix       => "Journal Voucher";
    }
}
