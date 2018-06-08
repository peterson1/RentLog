using CommonTools.Lib11.DTOs;
using System;
using System.Collections.Generic;

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


        public class AccountAllocation
        {
            public GLAccountDTO  Account    { get; set; }
            public decimal       SubAmount  { get; set; }
        }
    }
}
