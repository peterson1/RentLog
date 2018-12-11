using CommonTools.Lib11.StringTools;
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
        private BillAmounts _totals;


        public WithBackRentsOrRightsVM(MainWindowVM main, ITenantDBsDir dir) : base(main, dir)
        {
        }


        protected override List<LeaseBalanceRow> GetCacheableList()
        {
            if (PickedSection.Id == 0)
                return AppArgs.Balances.GetOverdueLeases(out _totals)
                                       .OrderByDescending(_ => _.Rent)
                                       .ToList();
            else
                return AppArgs.Balances.GetOverdueLeases(out BillAmounts totals, PickedSection)
                                       .OrderByDescending(_ => _.Rent)
                                       .ToList();
        }


        public override string TopRightText => $"Total Back Rent :  {_totals?.Rent:N2}" + L.f
             + $"Total Overdue Rights :  {_totals?.Rights:N2}";


        protected override List<LeaseDTO> GetLeases(MarketStateDbBase mkt, int secId)
            => throw new NotImplementedException();
    }
}
