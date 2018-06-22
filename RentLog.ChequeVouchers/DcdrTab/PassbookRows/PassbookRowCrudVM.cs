using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using System;

namespace RentLog.ChequeVouchers.DcdrTab.PassbookRows
{
    [AddINotifyPropertyChangedInterface]
    public class PassbookRowCrudVM : RepoCrudWindowVMBase<IPassbookRowsRepo, PassbookRowDTO, PassbookRowCrudWindow, ITenantDBsDir>
    {
        public PassbookRowCrudVM(IPassbookRowsRepo repository, ITenantDBsDir appArguments) : base(repository, appArguments)
        {
        }


        public DateTime TransactionDate { get; set; }


        protected override void ModifyDraftForInserting(PassbookRowDTO draft)
        {
            TransactionDate  = DateTime.Now;
            draft.DocRefType = draft.GetType().FullName;
            draft.DocRefJson = draft.ToJson(true);
        }


        protected override void ModifyDraftForUpdating(PassbookRowDTO draft)
        {
            TransactionDate = DateTime.MinValue.AddDays(draft.DateOffset);
        }


        public void OnTransactionDateChanged()
        {
            if (Draft != null)
                Draft.DateOffset = TransactionDate.DaysSinceMin();
        }


        public override string    TypeDescription => "Bank Transaction";
        protected override string CaptionPrefix   => "Bank Transaction";
    }
}
