﻿using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.PassbookRepos
{
    public interface IPassbookRowsRepo : ISimplerRepo<PassbookRowDTO>//, IReadOnlyRepo<PassbookRowDTO>
    {
        int   BankAccountID         { get; }
        void  InsertClearedCheque   (ChequeVoucherDTO cheque, DateTime clearedDate);
        void  InsertDepositedColxn  (BankDepositDTO bankDeposit, DateTime colxnDate);
        void  RecomputeBalancesFrom (DateTime date);

        PassbookRowDTO  FindByDocRefId  (int docRefId, DateTime transactionDate);

        List<PassbookRowDTO> RowsFor (DateTime dateTime);
        List<PassbookRowDTO> RowsFor (DateTime startDate, DateTime endDate);
    }
}
