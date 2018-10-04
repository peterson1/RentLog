using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System.Linq;

namespace RentLog.DomainLib11.AdHocJobs
{
    public static class EnsureGLAccount
    {
        public static void ElectricWaterDeposit(ITenantDBsDir dir)
            => dir.AddIfNone("Electric/Water Deposit", GLAcctType.Liability);


        public static void MeralcoDeposit(ITenantDBsDir dir)
            => dir.AddIfNone("Meralco - Deposit", GLAcctType.Asset);


        public static void RightsRefund(ITenantDBsDir dir)
            => dir.AddIfNone("Rights Refund", GLAcctType.Expense);


        public static void AR_BK_Garay(ITenantDBsDir dir)
            => dir.AddIfNone("A/R BK Norzagaray", GLAcctType.Asset);


        public static void All_AR_Others(ITenantDBsDir dir)
        {
            dir.AddIfNone("A/R Others - Fortune Balagtas", GLAcctType.Asset);
            dir.AddIfNone("A/R Others - Fortune Marilao", GLAcctType.Asset);
            dir.AddIfNone("A/R Others - Fortune Norzagaray", GLAcctType.Asset);
            dir.AddIfNone("A/R Others - Fortune Meycauayan", GLAcctType.Asset);
            dir.AddIfNone("A/R Others - Fortune Bignay", GLAcctType.Asset);
            dir.AddIfNone("A/R Others - BK Balagtas", GLAcctType.Asset);
            dir.AddIfNone("A/R Others - BK Meycauayan", GLAcctType.Asset);
            dir.AddIfNone("A/R Others - BK Bignay", GLAcctType.Asset);
        }


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
