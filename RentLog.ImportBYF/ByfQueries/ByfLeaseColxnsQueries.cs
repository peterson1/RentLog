using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfLeaseColxnsQueries
    {
        public const string PUBLISHED_LEASE_COLXNS = "daily_collections_repo?display_id=page_2";


        public static async Task<decimal> GetLeaseColxnsTotal(this ByfClient1 client, DateTime date)
        {
            var dynamics = await client.GetViewsList(PUBLISHED_LEASE_COLXNS, date);
            return dynamics.Select(_ => GetSubTotal(_)).Sum(_ => _);
        }


        private static decimal GetSubTotal(dynamic byf)
            => As.Decimal(byf.rent)
             + As.Decimal(byf.rights)
             + As.Decimal(byf.electric)
             + As.Decimal(byf.water)
             + As.Decimal(byf.surcharge);
    }
}
