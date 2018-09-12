﻿using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class LeaseDTO : DocumentDTOBase
    {
        public TenantModel   Tenant                { get; set; }
        public StallDTO      Stall                 { get; set; }
        public DateTime      ContractStart         { get; set; }
        public DateTime      ContractEnd           { get; set; }
        public DateTime      ApplicationSubmitted  { get; set; }
        public RentParams    Rent                  { get; set; }
        public RightsParams  Rights                { get; set; }
        public string        ProductToSell         { get; set; }


        public string   TenantAndStall    => $"{Tenant} : {Stall}";
        public DateTime FirstRentDueDate  => ContractStart.AddDays(Rent?.GracePeriodDays ?? 0);
        public DateTime RightsDueDate     => ContractStart.AddDays(Rights?.SettlementDays ?? 0);
        public int      ContractSpanDays  => (int)(ContractEnd - ContractStart).TotalDays;
        public override string ToString() => TenantAndStall;
    }
}
