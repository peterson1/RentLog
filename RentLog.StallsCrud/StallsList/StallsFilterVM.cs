using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.StallsCrud.StallsList
{
    class StallsFilterVM : TextFilterBase<StallDTO>
    {
        protected override Dictionary<string, Func<StallDTO, string>> FilterProperties => new Dictionary<string, Func<StallDTO, string>>
        {
        };
    }
}
