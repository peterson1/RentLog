using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.FilteredLeases.FilteredLists.AllInactiveLeases
{
    public class AllInactiveLeasesVM : FilteredListVMBase
    {
        public AllInactiveLeasesVM(ITenantDBsDir dir) : base(dir)
        {
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt)
            => mkt.InactiveLeases
                  .GetAll()
                  .Select(_ => _ as LeaseDTO)
                  .ToList();
    }
}
