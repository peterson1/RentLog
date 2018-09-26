using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Environment;

namespace RentLog.ImportBYF.Converters
{
    public abstract class ComparisonsListBase : UIList<JsonComparer>
    {
        private const string CACHE_DIR = "BasicAuthBulkCacheReader";


        internal abstract List<object> QueryRNT(ITenantDBsDir appArgs);
        internal abstract List<object> QueryBYF(string cacheDir);


        public void ReloadList(ITenantDBsDir dir)
        {
            var tupl  = QueryBothSources(dir);
            var items = GetComparisons(tupl);

            UIThread.Run(() => SetItems(items));
        }


        private (List<object>, List<object>) QueryBothSources(ITenantDBsDir dir)
        {
            List<object> byfs  = null;
            List<object> rnts  = null;
            Parallel.Invoke(() => byfs = QueryBYF(FindCacheDir()),
                            () => rnts = QueryRNT(dir));
            return (byfs, rnts);
        }


        private IEnumerable<JsonComparer> GetComparisons((List<object>, List<object>) tupl)
        {
            throw new NotImplementedException();
        }


        private string FindCacheDir()
            => SpecialFolder.LocalApplicationData.Path(CACHE_DIR);
    }
}
