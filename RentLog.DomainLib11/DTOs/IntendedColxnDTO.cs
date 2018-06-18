using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.DTOs
{
    public class IntendedColxnDTO : UncollectedLeaseDTO
    {
        public int           PRNumber   { get; set; }
        public BillAmounts   Actuals    { get; set; }


        public override string ToString()
            => $"‹Colxn› from [{Lease}]";
    }
}
