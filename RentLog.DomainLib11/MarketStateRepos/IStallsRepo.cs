using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public interface IStallsRepo : ISimpleRepo<StallDTO>
    {
        List<StallDTO> ForSection(SectionDTO section);
    }
}
