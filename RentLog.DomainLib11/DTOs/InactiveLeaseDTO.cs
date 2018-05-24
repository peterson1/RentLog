using CommonTools.Lib11.ReflectionTools;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class InactiveLeaseDTO : LeaseDTO
    {
        public InactiveLeaseDTO()
        {
        }

        public InactiveLeaseDTO(LeaseDTO lse, string reason, DateTime deactivationDate, string deactivatedBy)
        {
            this.CopyByNameFrom(lse);
            WhyInactive     = reason;
            DeactivatedDate = deactivationDate;
            DeactivatedBy   = deactivatedBy;
        }


        public DateTime   DeactivatedDate  { get; set; }
        public string     DeactivatedBy    { get; set; }
        public string     WhyInactive      { get; set; }
    }
}
