using CommonTools.Lib11.DynamicTools;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.MiscellaneousConverters
{
    public static class FiscalDaysConverter1
    {
        public const string ViewsDisplayID = "all_fiscal_days?display_id=page_3";


        public static async Task<DateTime> GetLastPostedDate(this IConverterRow conv)
        {
            var dynamics = await conv.GetViewsList(ViewsDisplayID);
            var item1    = dynamics.First();
            return As.Date(item1.max);
        }
    }
}
