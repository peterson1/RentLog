﻿using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.ReportRows
{
    public class LeaseColxnRow
    {
        public LeaseColxnRow(IntendedColxnDTO dto)
        {
            Lease       = dto.Lease;
            DocumentRef = dto.PRNumber.ToString();
            Rent        = dto.Actuals.Rent;
            Rights      = dto.Actuals.Rights;
            Electric    = dto.Actuals.Electric;
            Water       = dto.Actuals.Water;
            Remarks     = dto.Remarks;
        }


        public LeaseColxnRow(CashierColxnDTO dto)
        {
            Lease       = dto.Lease;
            DocumentRef = dto.DocumentRef;
            Rent        = dto.For(BillCode.Rent    );
            Rights      = dto.For(BillCode.Rights  );
            Electric    = dto.For(BillCode.Electric);
            Water       = dto.For(BillCode.Water   );
            Remarks     = dto.Remarks;
        }


        public LeaseColxnRow(SectionDTO sec, AmbulantColxnDTO dto)
        {
            Lease = new LeaseDTO
            {
                Tenant = TenantModel.Named(dto.ReceivedFrom),
                Stall  = StallDTO.Named($"{sec.Name} Section Ambulant")
            };
            DocumentRef = dto.PRNumber?.ToString();
            Ambulant    = dto.Amount;
            Remarks     = dto.Remarks;
        }


        public LeaseDTO     Lease         { get; set; }
        public string       DocumentRef   { get; set; }
        public decimal?     Rent          { get; set; }
        public decimal?     Rights        { get; set; }
        public decimal?     Electric      { get; set; }
        public decimal?     Water         { get; set; }
        public decimal?     Ambulant      { get; set; }
        public string       Remarks       { get; set; }
    }
}