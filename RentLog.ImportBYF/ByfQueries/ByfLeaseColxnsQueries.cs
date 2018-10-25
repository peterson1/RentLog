using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfLeaseColxnsQueries
    {
        private const string PUBLISHED_LEASE_COLXNS = "daily_collections_repo?display_id=page_2";


        public static async Task<decimal> GetLeaseColxnsTotal(this ByfClient1 client, DateTime date)
        {
            var dynamics = await client.GetRawByfLeaseColxns(date);
            var total = 0M;

            foreach (var byf in dynamics)
                total += GetSubTotal(byf);

            return total;
        }


        public static Task<List<dynamic>> GetRawByfLeaseColxns(this ByfClient1 client, DateTime date)
            => client.GetViewsList(PUBLISHED_LEASE_COLXNS, date);


        private static decimal GetSubTotal(dynamic byf)
            => As.Decimal(byf.rent)
             + As.Decimal(byf.rights)
             + As.Decimal(byf.electric)
             + As.Decimal(byf.water)
             + As.Decimal(byf.surcharge);
    }
}
