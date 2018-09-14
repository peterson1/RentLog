using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.FilteredLeases.FilteredLists.WithBackRentsOrRights
{
    public class WithBackRentsOrRightsVM : FilteredListVMBase
    {
        public WithBackRentsOrRightsVM(ITenantDBsDir dir) : base(dir)
        {
        }


        protected override List<LeaseBalanceRow> GetCacheableList()
            => AppArgs.Balances.GetOverdueLeases(out BillAmounts totals)
                               .OrderByDescending(_ => _.Rent)
                               .ToList();


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt)
            => throw new NotImplementedException();
    }
}
