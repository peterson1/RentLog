using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.DomainLib11.Repositories
{
    public interface IStallsRepo : ISimpleRepo<StallDTO>
    {
        List<StallDTO> ForSection(SectionDTO section);
    }
}
