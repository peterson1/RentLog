using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfAmbulantColxnsQueries
    {
        public const string PUBLISHED_AMBULANT_COLXNS = "other_collections_list?display_id=page_2";


        public static async Task<decimal> GetAmbulantColxnsTotal(this ByfClient1 client, DateTime date)
        {
            var dynamics = await client.GetViewsList(PUBLISHED_AMBULANT_COLXNS, date);
            return dynamics.Select(_ => GetAmount(_)).Sum(_ => _);
        }


        public static decimal GetAmount (dynamic byf)
        {
            if (!IsValidRemarks(byf)) return 0;
            return As.Decimal(byf.amount);
        }


        private static bool IsValidRemarks(dynamic byf)
        {
            var rem = As.Text(byf.remarks);
            if ( rem.IsBlank()) return false;
            if (!rem.Contains("{")) return false;
            if (!rem.Contains(":")) return false;
            if (!rem.Contains("}")) return false;
            return true;
        }
    }
}
