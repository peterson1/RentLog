using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.Validations
{
    public static class PassbookRowValidations
    {
        public static string WhyInvalidForSetAsCleared(this ChequeVoucherDTO chequeVoucher, int bankAcctID)
        {
            var req = chequeVoucher.Request;
            if (req == null)
                return "Request object should not be NULL.";

            if (req.BankAccountId != bankAcctID)
                return $"Expected Bank Acct ID to be [{bankAcctID}] but was [{req.BankAccountId}]";

            if (!req.Amount.HasValue)
                return "Requested amount should not be blank.";

            return string.Empty;
        }


        public static string WhyInvalidForColxnDeposit(this BankDepositDTO deposit, int bankAcctID)
        {
            if (deposit.BankAccount.Id != bankAcctID)
                return $"Expected Bank Acct ID to be [{bankAcctID}] but was [{deposit.BankAccount.Id}]";

            if (deposit.DepositDate == DateTime.MinValue)
                return "[DepositDate] should not be blank.";

            if (deposit.Amount < 0)
                return "Deposit amount should be greater than zero.";

            return string.Empty;
        }
    }
}
