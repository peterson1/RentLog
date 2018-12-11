using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.PassbookRepos
{
    public interface IPassbookDB
    {
        IPassbookRowsRepo     GetRepo             (int bankAccountId);
        List<PassbookRowDTO>  RowsFromAllAccounts (DateTime startDate, DateTime endDate, MarketStateDbBase marketStateDB);
    }
}
