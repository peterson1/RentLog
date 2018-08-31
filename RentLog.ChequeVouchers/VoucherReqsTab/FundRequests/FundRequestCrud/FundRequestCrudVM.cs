using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud.AllocationsList;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud
{
    [AddINotifyPropertyChangedInterface]
    public class FundRequestCrudVM : RepoCrudWindowVMBase<IFundRequestsRepo, FundRequestDTO, FundRequestCrudWindow, ITenantDBsDir>
    {
        public FundRequestCrudVM(ITenantDBsDir dir) : base(dir.Vouchers.ActiveRequests, dir)
        {
            Allocations = new AllocationsListVM();
        }

        public DateTime           CrudRequestDate { get; set; }
        public UIList<string>     Payees          { get; } = new UIList<string>();
        public AllocationsListVM  Allocations     { get; }


        protected override void ModifyDraftForInserting(FundRequestDTO draft)
        {
            draft.BankAccountId = AppArgs.CurrentBankAcct.Id;
            //CrudRequestDate     = AppArgs.Vouchers.GetNextRequestDate();
            CrudRequestDate     = DateTime.Now.Date;
            draft.SerialNum     = AppArgs.Vouchers.GetNextRequestSerial();
            draft.Allocations   = new List<AccountAllocation>();

            Allocations.SetHost(draft.Allocations, AppArgs.CurrentBankAcct, AppArgs.MarketState.GLAccounts);
        }


        public void OnCrudRequestDateChanged()
        {
            if (Draft == null) return;
            Draft.DateOffset = CrudRequestDate.DaysSinceMin();
        }


        protected override void ModifyDraftForUpdating(FundRequestDTO draft)
        {
            CrudRequestDate = draft.RequestDate;
            Allocations.SetHost(draft.Allocations, AppArgs.CurrentBankAcct, AppArgs.MarketState.GLAccounts);
            Allocations.OnAmountChanged(draft.Amount);
        }


        protected override void UpdateRecord(FundRequestDTO record)
        {
            var match = _repo.Find(record.Id, false);
            if (match == null)
                AppArgs.Vouchers.InactiveRequests.Update(record);
            else
                base.UpdateRecord(record);
        }


        protected override async void OnWindowLoaded()
        {
            if (Draft.Id != 0) return;
            List<string> items = null;
            await Task.Run(() => items = AppArgs.Vouchers.GetPayees());
            UIThread  .Run(() => Payees.SetItems(items));
        }


        protected override string CaptionPrefix       => "Voucher Request";
        public    override string TypeDescription     => "Voucher Request";
        public    override bool   CanEncodeNewDraft() => AppArgs.CanAddVoucherRequest(false);
    }
}
