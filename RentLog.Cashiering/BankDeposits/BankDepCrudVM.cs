using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;
using static RentLog.Cashiering.Properties.Settings;

namespace RentLog.Cashiering.BankDeposits
{
    public class BankDepCrudVM : RepoCrudWindowVMBase<IBankDepositsRepo, BankDepositDTO, BankDepositEditWindow, ITenantDBsDir>
    {
        public BankDepCrudVM(IBankDepositsRepo repository, ITenantDBsDir dir) : base(repository, dir)
        {
            BankAccounts.SetItems(dir.MarketState.BankAccounts.GetAll());
            Descriptions.SetItems(Default.DepositDescriptions.Cast<string>());
        }


        public UIList<BankAccountDTO>  BankAccounts  { get; } = new UIList<BankAccountDTO>();
        public UIList<string>          Descriptions  { get; } = new UIList<string>();


        protected override BankDepositDTO GetNewDraft()
            => new BankDepositDTO
            {
                Description = Descriptions.First(),
                DepositDate = DateTime.Now.AddDays(-1).Date,
            };


        protected override void ModifyDraftForUpdating(BankDepositDTO draft)
            => draft.BankAccount = BankAccounts.SingleOrDefault(_ => _.Id == draft.BankAccount.Id);


        public    override string TypeDescription => "Bank Deposit";
        protected override string CaptionPrefix   => "Bank Deposit";
    }
}
