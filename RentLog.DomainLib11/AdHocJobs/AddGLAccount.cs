using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System.Linq;

namespace RentLog.DomainLib11.AdHocJobs
{
    public static class EnsureGLAccount
    {
        public static void MeralcoDeposit(ITenantDBsDir dir)
            => dir.AddIfNone("Meralco - Deposit", GLAcctType.Asset);


        public static void RightsRefund(ITenantDBsDir dir)
            => dir.AddIfNone("Rights Refund", GLAcctType.Expense);


        private static void AddIfNone(this ITenantDBsDir dir, string glAcctName, GLAcctType glAcctType)
        {
            var repo    = dir.MarketState.GLAccounts;
            var matches = repo.Find(_ => _.Name == glAcctName);
            if (matches.Any()) return;
            repo.Insert(new GLAccountDTO
            {
                Name        = glAcctName,
                AccountType = glAcctType
            });
        }
    }
}
