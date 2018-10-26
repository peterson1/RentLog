using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.StringTools;
using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfAmbulantColxnsQueries
    {
        public const string PUBLISHED_AMBULANT_COLXNS = "other_collections_list?display_id=page_2";


        public static async Task<decimal> GetAmbulantColxnsTotal(this ByfClient1 client, DateTime date)
        {
            var dynamics = await client.GetRawByfAmbulantColxns(date);
            var total = 0M;

            foreach (var byf in dynamics)
                total += As.Decimal(byf.amount);

            return total;
        }


        public static bool IsCompositeRemarks(dynamic byf)
        {
            var rem = (string)As.Text(byf.remarks);
            if ( rem.IsBlank()) return false;
            if (!rem.Contains("{")) return false;
            if (!rem.Contains(":")) return false;
            if (!rem.Contains("}")) return false;
            return true;
        }


        public static async Task<List<dynamic>> GetRawByfAmbulantColxns(this ByfClient1 client, DateTime date)
        {
            var list = await client.GetViewsList(PUBLISHED_AMBULANT_COLXNS, date);
            return list.Where(_ => IsCompositeRemarks(_)).ToList();
        }
    }
}
