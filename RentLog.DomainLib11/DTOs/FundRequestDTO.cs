using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public enum ChequeState
    {
        Prepared,
        Issued,
        Cleared,
        Cancelled
    }


    public class FundRequestDTO : WithAllocations
    {
        public int           SerialNum      { get; set; }
        public int           BankAccountId  { get; set; }
        public string        Payee          { get; set; }
        public string        Purpose        { get; set; }
        public int           DateOffset     { get; set; }
        public decimal?      Amount         { get; set; }
        public ChequeState?  ChequeStatus   { get; set; }


        public DateTime RequestDate 
            => DateTime.MinValue.AddDays(DateOffset);
        //public DateTime RequestDate { get; set; }


        public override string ToString()
            => $"CV# {SerialNum:0000} for “{Payee}”";
    }
}
