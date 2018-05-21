using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.ReportRows
{
    public class LeaseBalanceRow : BillAmounts
    {
        public LeaseBalanceRow(LeaseDTO lease)
        {
            Lease = lease;
        }


        public LeaseDTO  Lease     { get; }
        public bool      IsActive  => !(Lease is InactiveLeaseDTO);
    }
}
