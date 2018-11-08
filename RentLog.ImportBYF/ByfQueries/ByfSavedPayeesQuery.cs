using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.ByfServerAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfSavedPayeesQuery
    {
        private const string VIEWS_ID = "payees_list?display_id=page_2";


        public static async Task<Dictionary<int, string>> GetPayeesDictionary(this ByfClient1 client)
        {
            var dynamics = await client.GetRawByfPayees();
            var dict     = new Dictionary<int, string>();

            foreach (var byf in dynamics)
                dict.Add(As.ID  (byf.nid),
                         As.Text(byf.name));
            return dict;
        }


        public static Task<List<dynamic>> GetRawByfPayees(this ByfClient1 client)
            => client.GetViewsList(VIEWS_ID);
    }
}
