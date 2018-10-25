using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfTaxonomyTermsQueries
    {
        private const string VIEWS_ID = "taxo_terms?display_id=page";


        public static async Task<Dictionary<int, string>> GetTaxonomyDictionary(this ByfClient1 client)
        {
            var dynamics = await client.GetRawByfTaxonomyTerms();
            var dict     = new Dictionary<int, string>();

            foreach (var byf in dynamics)
                dict.Add(As.ID  (byf.tid),
                         As.Text(byf.name));
            return dict;
        }


        public static Task<List<dynamic>> GetRawByfTaxonomyTerms(this ByfClient1 client)
            => client.GetViewsList(VIEWS_ID);
    }
}
