using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class BankAccountsRepo1 : MarketStateRepoShimBase<BankAccountDTO>, IBankAccountsRepo
    {
        public BankAccountsRepo1(ISimpleRepo<BankAccountDTO> simpleRepo, MarketStateDB marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }

        protected override void ValidateBeforeInsert(BankAccountDTO newRecord)
            => this.RejectDuplicateRecord(_ => _.Name == newRecord.Name,
                    nameof(newRecord.Name), newRecord);


        protected override void ValidateBeforeUpdate(BankAccountDTO changedRecord)
            => this.RejectDuplicateRecord(_ => _.Name == changedRecord.Name,
                    nameof(changedRecord.Name), changedRecord, _ => _.Id);
    }
}
