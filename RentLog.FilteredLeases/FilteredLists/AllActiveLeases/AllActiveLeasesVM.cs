using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;

namespace RentLog.FilteredLeases.FilteredLists.AllActiveLeases
{
    public class AllActiveLeasesVM : FilteredListVMBase
    {
        public AllActiveLeasesVM(ITenantDBsDir dir) : base(dir)
        {
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt)
            => mkt.ActiveLeases.GetAll();
    }
}
