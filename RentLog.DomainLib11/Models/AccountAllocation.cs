using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.Models
{
    public class AccountAllocation
    {
        public GLAccountDTO   Account    { get; set; }
        public decimal        SubAmount  { get; set; }

        public bool IsDebit  => SubAmount < 0;
        public bool IsCredit => SubAmount > 0;

        public virtual decimal? AsDebit  => IsDebit ? SubAmount * -1M : (decimal?)null;
        public virtual decimal? AsCredit => IsCredit ? SubAmount : (decimal?)null;


        public static AccountAllocation NewDebit(GLAccountDTO glAccount, decimal unsignedAmount)
            => NewItem(glAccount, unsignedAmount, -1);

        public static AccountAllocation NewCredit(GLAccountDTO glAccount, decimal unsignedAmount)
            => NewItem(glAccount, unsignedAmount, +1);

        public static AccountAllocation DefaultCashInBank(BankAccountDTO bankAccount, decimal amount)
            => NewCredit(GLAccountDTO.CashInBank(bankAccount), amount);

        public static AccountAllocation NewItem(GLAccountDTO glAccount, decimal unsignedAmount, decimal multiplier) => new AccountAllocation
        {
            Account   = glAccount,
            SubAmount = Math.Abs(unsignedAmount) * multiplier
        };
    }
}
