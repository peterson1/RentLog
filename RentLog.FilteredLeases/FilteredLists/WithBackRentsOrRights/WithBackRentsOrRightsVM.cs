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
        public WithBackRentsOrRightsVM(MainWindowVM main, ITenantDBsDir dir) : base(main, dir)
        {
        }


        protected override List<LeaseBalanceRow> GetCacheableList()
        {
            if (PickedSection.Id == 0)
                return AppArgs.Balances.GetOverdueLeases(out BillAmounts totals)
                                       .OrderByDescending(_ => _.Rent)
                                       .ToList();
            else
                return AppArgs.Balances.GetOverdueLeases(out BillAmounts totals, PickedSection)
                                       .OrderByDescending(_ => _.Rent)
                                       .ToList();
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt, int secId)
            => throw new NotImplementedException();
    }
}
