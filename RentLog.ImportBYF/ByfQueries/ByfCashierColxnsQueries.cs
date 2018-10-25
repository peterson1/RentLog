using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfCashierColxnsQueries
    {
        public const string PUBLISHED_CASHIER_COLXNS = "balance_adjustments?display_id=page_2";


        public static async Task<decimal> GetCashierColxnsTotal(this ByfClient1 client, DateTime date)
        {
            var dynamics = await client.GetViewsList(PUBLISHED_CASHIER_COLXNS, date);
            //return dynamics.Select(_ => GetSubTotal(_)).Sum(_ => _);
            var total = 0M;

            foreach (var byf in dynamics)
                total += GetSubTotal(byf);

            return total;
        }


        private static decimal GetSubTotal(dynamic byf)
        {
            if (As.ID(byf.memotype) != 1) return 0;
            return As.Decimal(byf.rent)
                 + As.Decimal(byf.rights)
                 + As.Decimal(byf.electric)
                 + As.Decimal(byf.water)
                 + As.Decimal(byf.surcharge);
        }
    }
}
