using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfOtherColxnsQueries
    {
        public const string PUBLISHED_OTHER_COLXNS = "other_collections_list?display_id=page_2";


        public static async Task<decimal> GetOtherColxnsTotal(this ByfClient1 client, DateTime date)
        {
            var dynamics = await client.GetViewsList(PUBLISHED_OTHER_COLXNS, date);
            return dynamics.Select(_ => GetAmount(_)).Sum(_ => _);
        }


        public static decimal GetAmount(dynamic byf)
        {
            if (As.ID(byf.paymentfortid) != 32) return 0;
            return As.Decimal(byf.amount);
        }
    }
}
