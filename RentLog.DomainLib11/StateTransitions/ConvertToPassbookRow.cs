using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.JsonTools;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.StateTransitions
{
    public static class ConvertToPassbookRow
    {
        public static PassbookRowDTO ToPassbookRow
            (this ChequeVoucherDTO chq, DateTime clearedDate) => new PassbookRowDTO
        {
            DateOffset     = clearedDate.DaysSinceMin(),

            TransactionRef = $"{chq.ChequeNumber}",
            Subject        = $"Cheque for {chq.Request.Payee}",
            Description    = chq.Request.Purpose,
            Amount         = chq.Request.Amount.Value * -1M,

            DocRefType     = chq.GetType().FullName,
            DocRefId       = chq.Id,
            DocRefJson     = chq.ToJson(true),
        };


        public static PassbookRowDTO ToPassbookRow
            (this BankDepositDTO dep, DateTime colxnDate) => new PassbookRowDTO
        {
            DateOffset     = dep.DepositDate.DaysSinceMin(),

            TransactionRef = dep.DocumentRef,
            Subject        = "Cash Deposit",
            Description    = $"{colxnDate:MMM-d} Market Collections: {dep.Description}",
            Amount         = dep.Amount,

            DocRefType     = dep.GetType().FullName,
            DocRefId       = dep.Id,
            DocRefJson     = dep.ToJson(true),
        };
    }
}
