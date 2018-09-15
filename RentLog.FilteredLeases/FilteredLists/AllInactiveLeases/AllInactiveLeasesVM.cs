using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.FilteredLeases.FilteredLists.AllInactiveLeases
{
    public class AllInactiveLeasesVM : FilteredListVMBase
    {
        public AllInactiveLeasesVM(MainWindowVM main, ITenantDBsDir dir) : base(main, dir)
        {
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt, int secId)
        {
            var all = mkt.InactiveLeases.GetAll()
                         .Select (_ => _ as LeaseDTO)
                         .ToList ();
            return secId == 0 ? all
                : all.Where(_ => _.Stall.Section.Id == secId).ToList();
        }
    }
}
