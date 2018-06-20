using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using System;
using System.IO;

namespace RentLog.DatabaseLib.PassbookRowsRepository
{
    internal class TransactionsByMonthRepo : VagrantRepoFacadeBase
    {
        private const string DATE_FMT = "{0:yyyy-MM}_Txns_Shard.ldb";
        private string _dir;
        private string _usr;


        internal TransactionsByMonthRepo(int bankAccountId, string transactionsDir, string currentUser) : base(bankAccountId)
        {
            _dir = transactionsDir;
            _usr = currentUser;
        }


        protected override ISimpleRepo<PassbookRowDTO> ConnectToDB(string dbPath)
        {
            var txnDB  = new SharedLiteDB(dbPath, _usr);
            var colxn  = new PassbookRowCollection(BankAccountID, txnDB);
            return new PassbookRowsSimpleRepo(colxn);
        }


        protected override string GetDatabasePath(DateTime date)
            => Path.Combine(_dir, string.Format(DATE_FMT, date));
    }
}
