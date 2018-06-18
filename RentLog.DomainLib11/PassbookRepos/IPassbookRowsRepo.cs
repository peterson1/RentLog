using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.PassbookRepos
{
    public interface IPassbookRowsRepo
    {
        int   BankAccountID         { get; }
        void  InsertClearedCheque   (ChequeVoucherDTO cheque, DateTime clearedDate);
        void  InsertDepositedColxn  (BankAccountDTO bankDeposit);
        void  RecomputeBalancesFrom (DateTime date);
    }
}
