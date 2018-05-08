using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.ReportRows
{
    public class ActiveLeaseRow : IHasDTO<LeaseDTO>
    {
        public ActiveLeaseRow(LeaseDTO leaseDTO)
        {
            DTO = leaseDTO;
        }


        public LeaseDTO  DTO            { get; }
        public decimal?  RentBalance    { get; set; }
        public decimal?  RightsBalance  { get; set; }
    }
}
