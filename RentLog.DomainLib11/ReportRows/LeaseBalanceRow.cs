using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.ReportRows
{
    public class LeaseBalanceRow : BillAmounts, IHasDTO<LeaseDTO>, IHasDTO<InactiveLeaseDTO>
    {
        public LeaseBalanceRow(LeaseDTO lease, DailyBillDTO bill)
        {
            DTO      = lease;
            Rent     = bill?.For(BillCode.Rent    )?.ClosingBalance;
            Rights   = bill?.For(BillCode.Rights  )?.ClosingBalance;
            Electric = bill?.For(BillCode.Electric)?.ClosingBalance;
            Water    = bill?.For(BillCode.Water   )?.ClosingBalance;
        }


        public LeaseDTO   DTO   { get; }

        public bool             IsActive         => !(DTO is InactiveLeaseDTO);
        public InactiveLeaseDTO Inactive         => DTO as InactiveLeaseDTO;
        public DateTime?        DeactivatedDate  => Inactive?.DeactivatedDate;
        public string           DeactivatedBy    => Inactive?.DeactivatedBy ;
        public string           WhyInactive      => Inactive?.WhyInactive;
        public string           StatusText       => IsActive ? "Active" : "(terminated)";
        public string           CompositeRemarks => " -- ".JoinNonBlanks(DTO.Remarks,
                                                                         DTO.Tenant.Remarks,
                                                                         WhyInactive);


        InactiveLeaseDTO IHasDTO<InactiveLeaseDTO>.DTO => Inactive;
    }
}
