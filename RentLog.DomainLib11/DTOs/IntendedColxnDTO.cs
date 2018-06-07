using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.DTOs
{
    public class IntendedColxnDTO : DocumentDTOBase
    {
        public int           PRNumber   { get; set; }
        public LeaseDTO      Lease      { get; set; }
        public BillAmounts   Actuals    { get; set; }
        public BillAmounts   Targets    { get; set; }


        public override string ToString()
            => $"‹Colxn› from [{Lease}]";
    }
}
