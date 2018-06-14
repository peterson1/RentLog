using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud.AllocationsList;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using static RentLog.DomainLib11.DTOs.FundRequestDTO;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud
{
    [AddINotifyPropertyChangedInterface]
    public class FundRequestCrudVM : RepoCrudWindowVMBase<IFundRequestsRepo, FundRequestDTO, FundRequestCrudWindow, ITenantDBsDir>
    {
        public FundRequestCrudVM(ITenantDBsDir dir) : base(dir.Vouchers.ActiveRequests, dir)
        {
            Allocations = new AllocationsListVM();
        }

        public UIList<string>     Payees       { get; } = new UIList<string>();
        public AllocationsListVM  Allocations  { get; }


        protected override void ModifyDraftForInserting(FundRequestDTO draft)
        {
            draft.BankAccountId = AppArgs.CurrentBankAcct.Id;
            draft.RequestDate   = AppArgs.Vouchers.GetNextRequestDate();
            draft.SerialNum     = AppArgs.Vouchers.GetNextRequestSerial();
            draft.Allocations   = new List<AccountAllocation>();

            Allocations.SetHost(draft.Allocations, AppArgs.CurrentBankAcct);
        }


        protected override async void OnWindowLoaded()
        {
            List<string> items = null;
            await Task.Run(() => items = AppArgs.Vouchers.GetPayees());
            UIThread  .Run(() => Payees.SetItems(items));
        }


        protected override string CaptionPrefix       => "Voucher Request";
        public    override string TypeDescription     => "Voucher Request";
        public    override bool   CanEncodeNewDraft() => AppArgs.CanAddVoucherRequest(false);
    }
}
