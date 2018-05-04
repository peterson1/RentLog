using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.StallsCrud.StallsList
{
    public class StallRowVM : IHasDTO<StallDTO>
    {
        public StallRowVM(StallDTO dto)
        {
            DTO = dto;
            
        }


        public StallDTO  DTO       { get; }
        public LeaseDTO  Occupant  { get; set; }
    }
}
