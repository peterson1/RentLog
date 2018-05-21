using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.ReportRows
{
    public class LeaseBalanceRow : BillAmounts, IHasDTO<LeaseDTO>
    {
        public LeaseBalanceRow(LeaseDTO lease)
        {
            DTO = lease;
        }


        public LeaseDTO  DTO      { get; }
        public bool      IsActive  => !(DTO is InactiveLeaseDTO);
    }
}
