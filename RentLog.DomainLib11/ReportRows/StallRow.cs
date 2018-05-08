using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.ReportRows
{
    public class StallRow : IHasDTO<StallDTO>
    {
        public StallRow(StallDTO dto)
        {
            DTO = dto;
        }


        public StallDTO  DTO       { get; }
        public LeaseDTO  Occupant  { get; set; }
    }
}
