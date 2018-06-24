using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class JournalVoucherDTO : WithAllocations
    {
        public int       SerialNum     { get; set; }
        public int       DateOffset    { get; set; }
        public string    Description   { get; set; }
        public decimal   Amount        { get; set; }


        public DateTime TransactionDate => DateTime.MinValue.AddDays(DateOffset);
    }
}
