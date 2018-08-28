﻿using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class FundRequestDTO : WithAllocations
    {
        public int        SerialNum      { get; set; }
        public int        BankAccountId  { get; set; }
        public string     Payee          { get; set; }
        public string     Purpose        { get; set; }
        public int        DateOffset     { get; set; }
        public decimal?   Amount         { get; set; }


        public DateTime RequestDate 
            => DateTime.MinValue.AddDays(DateOffset);


        public override string ToString()
            => $"CV# {SerialNum:0000} for “{Payee}”";
    }
}
