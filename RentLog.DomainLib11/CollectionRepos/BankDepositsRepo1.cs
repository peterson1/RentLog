using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class BankDepositsRepo1 : SimpleRepoShimBase<BankDepositDTO>, IBankDepositsRepo
    {
        public BankDepositsRepo1(ISimpleRepo<BankDepositDTO> simpleRepo) : base(simpleRepo)
        {
        }


        protected override string GetWhyInvalid(BankDepositDTO dto)
        {
            if (dto.BankAccount == null)
                return "Bank Account should have a value.";

            if (dto.Amount <= 0)
                return "Amount should be greater than zero.";

            if (dto.DocumentRef.IsBlank())
                return "Deposit slip # should not be blank.";

            return string.Empty;
        }
    }
}
