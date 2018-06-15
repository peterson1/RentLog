using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.DTOs
{
    public class FundRequestDTO : DocumentDTOBase
    {
        public int        SerialNum      { get; set; }
        public int        BankAccountId  { get; set; }
        public string     Payee          { get; set; }
        public string     Purpose        { get; set; }
        public DateTime   RequestDate    { get; set; }
        public decimal?   Amount         { get; set; }

        public List<AccountAllocation>  Allocations  { get; set; }

        public decimal TotalDebit  => Allocations?.Sum(_ => _.AsDebit  ?? 0) ?? 0;
        public decimal TotalCredit => Allocations?.Sum(_ => _.AsCredit ?? 0) ?? 0;
        public bool    IsBalanced  => TotalCredit == TotalDebit;
    }
}
