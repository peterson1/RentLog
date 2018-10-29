using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfCashierColxnsQueries
    {
        private const string PUBLISHED_CASHIER_COLXNS = "balance_adjustments?display_id=page_2";


        public static async Task<decimal> GetCashierColxnsTotal(this ByfClient1 client, DateTime date)
        {
            var dynamics = await client.GetRawByfCashierColxns(date);
            var total = 0M;

            foreach (var byf in dynamics)
                total += As.DecimalOrZero(byf.rent     )
                       + As.DecimalOrZero(byf.rights   )
                       + As.DecimalOrZero(byf.electric )
                       + As.DecimalOrZero(byf.water    )
                       + As.DecimalOrZero(byf.surcharge);

            return total;
        }


        public static async Task<List<dynamic>> GetRawByfCashierColxns(this ByfClient1 client, DateTime date)
        {
            var list = await client.GetViewsList(PUBLISHED_CASHIER_COLXNS, date);
            return list.Where(_ => As.ID(_.memotype) == 1).ToList();
        }
    }
}
