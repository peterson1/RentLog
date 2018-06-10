using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.PassbookRepos
{
    public interface IPassbookRowsRepo
    {
        int BankAccountID { get; }
        void InsertClearedCheque(ChequeVoucherDTO cheque, DateTime date);
        void RecomputeBalancesFrom(DateTime date);
    }
}
