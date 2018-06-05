using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ReflectionTools;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class BankDepositDTO : DocumentDTOBase
    {
        public string             Description   { get; set; }
        public BankAccountDTO     BankAccount   { get; set; }
        public decimal            Amount        { get; set; }
        public DateTime           DepositDate   { get; set; }
        public string             DocumentRef   { get; set; }


        public override string ToString()
            => $"‹Bank Deposit› to {BankAccount}";
    }
}
