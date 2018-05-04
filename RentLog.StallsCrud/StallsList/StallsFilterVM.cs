using CommonTools.Lib45.BaseViewModels;
using System;
using System.Collections.Generic;

namespace RentLog.StallsCrud.StallsList
{
    class StallsFilterVM : TextFilterBase<StallRowVM>
    {
        public string Name { get; set; }

        protected override Dictionary<string, Func<StallRowVM, string>> FilterProperties => throw new NotImplementedException();
    }
}
