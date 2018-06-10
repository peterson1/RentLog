using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.JsonTools;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.StateTransitions
{
    public static class ChequeClearedToPassbook
    {
        public static PassbookRowDTO ToPassbookRow
            (this ChequeVoucherDTO chq, DateTime clearedDate) => new PassbookRowDTO
        {
            DateOffset     = clearedDate.DaysSinceMin(),

            TransactionRef = $"chq# {chq.ChequeNumber}",
            Subject        = $"Cheque for {chq.Request.Payee}",
            Description    = chq.Request.Purpose,
            Amount         = chq.Request.Amount.Value * -1,

            DocRefType     = chq.GetType().FullName,
            DocRefId       = chq.Id,
            DocRefJson     = chq.ToJson(true),
        };
    }
}
