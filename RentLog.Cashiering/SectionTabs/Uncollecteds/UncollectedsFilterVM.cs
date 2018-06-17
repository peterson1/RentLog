using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    public class UncollectedsFilterVM : TextFilterBase<UncollectedLeaseDTO>
    {

        public string NameAndStallFilter { get; set; }


        protected override Dictionary<string, Func<UncollectedLeaseDTO, string>> FilterProperties => new Dictionary<string, Func<UncollectedLeaseDTO, string>>
        {
            { nameof(NameAndStallFilter), _ => _.Lease.TenantAndStall }
        };
    }
}
