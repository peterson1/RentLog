using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.Validations
{
    public static class ChequeVoucherValidations
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
    }
}
