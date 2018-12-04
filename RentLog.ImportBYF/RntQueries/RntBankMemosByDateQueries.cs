using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.Version2UI.BankMemosPane.MemosByDateList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.RntQueries
{
    public static class RntBankMemosByDateQueries
    {
        public static MemosByDateCell GetBankMemos(this ITenantDBsDir dir, DateTime date)
        {
            var bankAccts = dir.MarketState.BankAccounts.GetAll();
            var dict      = new Dictionary<int, List<PassbookRowDTO>>();

            foreach (var bankAcct in bankAccts)
            {
                var repo  = dir.Passbooks.GetRepo(bankAcct.Id);
                var memos = repo.RowsFor(date)
                                .Where(_ => _.DocRefType == ByfBankMemoNode.TypeName)
                                .ToList();
                if (memos.Any())
                    dict[bankAcct.Id] = memos;
            }
            return new MemosByDateCell { DTOs = dict };
        }
    }
}
