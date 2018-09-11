using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.FilteredLeases.FilteredLists
{
    public class CommonTextFilterVM : TextFilterBase<LeaseBalanceRow>
    {
        protected override Dictionary<string, Func<LeaseBalanceRow, string>> FilterProperties => new Dictionary<string, Func<LeaseBalanceRow, string>>
        {

        };
    }
}
