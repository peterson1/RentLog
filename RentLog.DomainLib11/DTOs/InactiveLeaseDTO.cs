using System;

namespace RentLog.DomainLib11.DTOs
{
    public class InactiveLeaseDTO : LeaseDTO
    {
        public DateTime   DeactivatedDate  { get; set; }
        public string     DeactivatedBy    { get; set; }
        public string     WhyInactive      { get; set; }
    }
}
