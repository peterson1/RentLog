using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public class LeaseConverter1 : ComparisonsListBase
    {
        internal override List<object> QueryBYF(string cacheDir)
        {
            var dict = CacheReader2.getLeases(cacheDir);
            throw new NotImplementedException();
        }


        internal override List<object> QueryRNT(ITenantDBsDir appArgs)
        {
            throw new NotImplementedException();
        }
    }
}
