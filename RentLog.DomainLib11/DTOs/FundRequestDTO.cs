using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.DTOs
{
    public class FundRequestDTO : WithAllocations
    {
        public int        SerialNum      { get; set; }
        public int        BankAccountId  { get; set; }
        public string     Payee          { get; set; }
        public string     Purpose        { get; set; }
        public DateTime   RequestDate    { get; set; }
        public decimal?   Amount         { get; set; }


        public override string ToString()
            => $"CV# {SerialNum:0000} for “{Payee}”";
    }
}
