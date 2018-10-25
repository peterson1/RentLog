using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfBankDepositQueries
    {
        public const string PUBLISHED_BANK_DEPS = "bank_deposits_list?display_id=page_1";


        public static async Task<decimal> GetBankDepositsTotal(this ByfClient1 client, DateTime date)
        {
            var dynamics = await client.GetViewsList(PUBLISHED_BANK_DEPS, date);
            //return dynamics.Select(_ => As.Decimal(_.amount)).Sum(_ => _);
            var total = 0M;

            foreach (var byf in dynamics)
                total += As.Decimal(byf.amount);

            return total;
        }
    }
}
