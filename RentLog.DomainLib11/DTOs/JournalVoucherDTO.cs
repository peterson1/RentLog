using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.DTOs
{
    public class JournalVoucherDTO : WithAllocations
    {
        public int       SerialNum        { get; set; }
        public DateTime  TransactionDate  { get; set; }
        public string    Description      { get; set; }
        public decimal   Amount           { get; set; }
    }
}
