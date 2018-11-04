using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.FilteredLeases.FilteredLists.FullTenantDetails
{
    public class FullTenantDetailsVM : FilteredListVMBase
    {
        public FullTenantDetailsVM(MainWindowVM mainWindowVM, ITenantDBsDir dir) : base(mainWindowVM, dir)
        {
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt, int secId)
        {
            var all = mkt.GetAllLeases();
            return secId == 0 ? all
                : all.Where(_ => _.Stall.Section.Id == secId).ToList();
        }
    }
}
