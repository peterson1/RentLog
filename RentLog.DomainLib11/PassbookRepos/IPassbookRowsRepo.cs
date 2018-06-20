using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.PassbookRepos
{
    public interface IPassbookRowsRepo
    {
        int   BankAccountID         { get; }
        void  InsertClearedCheque   (ChequeVoucherDTO cheque, DateTime clearedDate);
        void  InsertDepositedColxn  (BankDepositDTO bankDeposit, DateTime colxnDate);
        void  RecomputeBalancesFrom (DateTime date);

        List<PassbookRowDTO> RowsFor (DateTime dateTime);
    }
}
