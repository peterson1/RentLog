using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib45.ThreadTools;

namespace RentLog.FilteredLeases.FilteredLists.AllActiveLeases
{
    public class AllActiveLeasesVM : FilteredListVMBase
    {
        public AllActiveLeasesVM(MainWindowVM main, ITenantDBsDir dir) : base(main, dir)
        {
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt, int secId)
        {
            var all  = mkt.ActiveLeases.GetAll();

            //MarketDayOpener.TerminateExpiredLeases(AppArgs);

            //var asOf = mkt.Collections.LastPostedDate();
            //foreach (var lse in all)
            //{
            //    if (!lse.IsActive(asOf))
            //        Alert.Show($"{lse}");
            //}

            return secId == 0 ? all
                : all.Where(_ => _.Stall.Section.Id == secId).ToList();
        }
    }
}
