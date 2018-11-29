using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public interface IInactiveLeasesRepo : ISimpleRepo<InactiveLeaseDTO>
    {
        List<InactiveLeaseDTO>  BySection  (int sectionId);
    }
}
