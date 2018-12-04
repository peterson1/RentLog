using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.PassbookRepos;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.Version2UI.BankMemosPane.MemosByDateList;
using System;

namespace RentLog.ImportBYF.RntCommands
{
    public static class RntUpdateBankMemosCommand
    {
        public static void UpdateBankMemos(this MemosByDateRow row)
        {
            var dir  = row.MainWindow.AppArgs;
            var pbks = dir.Passbooks;

            DeleteExistingRntRows(row.Date, pbks, dir);

            foreach (var kvp in row.ByfCell.DTOs)
            {
                var repo = pbks.GetRepo(kvp.Key);
                foreach (var dto in kvp.Value)
                    repo.Insert(dto);
            }
        }

        private static void DeleteExistingRntRows(DateTime date, IPassbookDB pbks, ITenantDBsDir dir)
        {
            var bankAccts = dir.MarketState.BankAccounts.GetAll();
            foreach (var bankAcct in bankAccts)
            {
                var repo  = pbks.GetRepo(bankAcct.Id);
                var memos = repo.RowsFor(date);
                foreach (var row in memos)
                {
                    if (row.DocRefType == ByfBankMemoNode.TypeName)
                        repo.Delete(row);
                }
            }
        }
    }
}
