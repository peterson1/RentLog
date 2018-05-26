using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class BankDepositsRepo1 : SimpleRepoShimBase<BankDepositDTO>, IBankDepositsRepo
    {
        public BankDepositsRepo1(ISimpleRepo<BankDepositDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
