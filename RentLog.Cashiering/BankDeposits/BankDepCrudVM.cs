using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;

namespace RentLog.Cashiering.BankDeposits
{
    public class BankDepCrudVM : RepoCrudWindowVMBase<IBankDepositsRepo, BankDepositDTO, BankDepositEditWindow, ITenantDBsDir>
    {
        public    override string TypeDescription => "Bank Deposit";
        protected override string CaptionPrefix   => "Bank Deposit";


        public BankDepCrudVM(IBankDepositsRepo repository, ITenantDBsDir dir) : base(repository, dir)
        {
            BankAccounts.SetItems(dir.MarketState.BankAccounts.GetAll());
        }


        public UIList<BankAccountDTO>  BankAccounts  { get; } = new UIList<BankAccountDTO>();
        public UIList<string>          Descriptions  { get; } = new UIList<string> { "Rental Collection", "Rights Collection", "Electricity Collection", "Parking Fees", "CR Fees", "Other Collections" };


        protected override BankDepositDTO GetNewDraft()
            => new BankDepositDTO
            {
                Description = Descriptions.First(),
                DepositDate = DateTime.Now.AddDays(-1).Date,
            };


        protected override void ModifyDraftForEditing(BankDepositDTO draft)
            => draft.BankAccount = BankAccounts.SingleOrDefault(_ => _.Id == draft.BankAccount.Id);


        protected override bool IsValidDraft(BankDepositDTO draft, out string whyInvalid)
        {
            if (draft.Amount <= 0)
            {
                whyInvalid = "Amount should be greater than zero.";
                return false;
            }

            if (draft.BankAccount == null)
            {
                whyInvalid = "Bank Account should have a value.";
                return false;
            }

            if (draft.DocumentRef.IsBlank())
            {
                whyInvalid = "Deposit slip # should not be blank.";
                return false;
            }

            whyInvalid = "";
            return true;
        }
    }
}
