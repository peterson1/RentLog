using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.DTOs
{
    public class UncollectedLeaseDTO : DocumentDTOBase
    {
        public LeaseDTO      Lease          { get; set; }
        public StallDTO      StallSnapshot  { get; set; }//workaround for reference being saved instead of whole object
        public BillAmounts   Targets        { get; set; }


        public override string ToString()
            => $"‹Uncollected› from [{Lease}]";
    }
}
