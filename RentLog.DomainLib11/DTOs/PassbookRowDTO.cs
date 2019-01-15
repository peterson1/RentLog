using CommonTools.Lib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.DTOs
{
    public class PassbookRowDTO : SerializedDocument
    {
        public string    Subject          { get; set; }
        public string    Description      { get; set; }
        public int       DateOffset       { get; set; }
        public string    TransactionRef   { get; set; }
        public decimal   Amount           { get; set; }
        public decimal   RunningBalance   { get; set; }//??

        public bool      IsDeposit       => Amount > 0;
        public bool      IsWithdrawal    => Amount < 0;
        public decimal?  Deposit         => IsDeposit ? Amount : (decimal?)null;
        public decimal?  Withdrawal      => IsWithdrawal ? Math.Abs(Amount) : (decimal?)null;
        public DateTime  TransactionDate => DateTime.MinValue.AddDays(DateOffset);

        public override bool Equals(object obj)
        {
            var dTO = obj as PassbookRowDTO;
            return dTO != null &&
                   Subject == dTO.Subject &&
                   Description == dTO.Description &&
                   DateOffset == dTO.DateOffset &&
                   TransactionRef == dTO.TransactionRef &&
                   Amount == dTO.Amount;
        }

        public override int GetHashCode()
        {
            var hashCode = 125899803;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Subject);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + DateOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TransactionRef);
            hashCode = hashCode * -1521134295 + Amount.GetHashCode();
            return hashCode;
        }


        //public static PassbookRowDTO Deposit(DateTime transactionDate, 
        //    string subject, string description, decimal amount, string transactionRef)
        //        => GetSoaRowDTO(transactionDate, subject, description, amount, 
        //                transactionRef, "Deposit", 1);


        //public static PassbookRowDTO Withdrawal(DateTime transactionDate,
        //    string subject, string description, decimal amount, string transactionRef)
        //        => GetSoaRowDTO(transactionDate, subject, description, amount, 
        //                transactionRef, "Withdrawal", -1);


        //private static PassbookRowDTO GetSoaRowDTO(DateTime transactionDate, 
        //    string subject, string description, decimal amount,
        //    string transactionRef, string txnType, decimal multiplier)
        //{
        //    if (amount <= 0) throw Bad.Arg($"{txnType} Amount", amount);
        //    return new PassbookRowDTO
        //    {
        //        DateOffset     = transactionDate.DaysSinceMin(),
        //        Subject        = subject,
        //        Description    = description,
        //        Amount         = amount * multiplier,
        //        TransactionRef = transactionRef
        //    };
        //}
    }
}
