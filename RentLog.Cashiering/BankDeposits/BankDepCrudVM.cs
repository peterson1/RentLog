using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.Models;

namespace RentLog.Cashiering.BankDeposits
{
    public class BankDepCrudVM : RepoCrudWindowVMBase<IBankDepositsRepo, BankDepositDTO, BankDepositEditWindow, ITenantDBsDir>
    {
        public    override string TypeDescription => "Bank Deposit";
        protected override string CaptionPrefix   => "Bank Deposit";


        public BankDepCrudVM(IBankDepositsRepo repository, ITenantDBsDir dir) : base(repository, dir)
        {
            //todo: set bank accts
            //BankAccounts.SetItems(dir.MarketState.b)
        }


        public UIList<BankAccountModel>  BankAccounts  { get; } = new UIList<BankAccountModel>();
        public UIList<string>            Descriptions  { get; } = new UIList<string> { "Rental Collection", "Rights Collection", "Electricity Collection", "Parking Fees", "CR Fees", "Other Collections" };


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
                whyInvalid = "Document Ref # should not be blank.";
                return false;
            }

            whyInvalid = "";
            return true;
        }
    }
}
