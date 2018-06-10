using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.PassbookRowsRepository;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.PassbookRepos;
using System;
using System.IO;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class TransactionsByMonthDir : IPassbookDB
    {
        private const string DIR_NAME = "Transactions";
        //private MarketStateDB _mkt;
        private string _dir;
        private string _usr;


        public TransactionsByMonthDir(MarketStateDB marketStateDB)
        {
            //_mkt = marketStateDB;
            _dir = GetTransactionsDir(marketStateDB.DatabasePath);
            _usr = marketStateDB.CurrentUser;
        }


        public IPassbookRowsRepo GetRepo(int bankAccountId) 
            => new TransactionsByMonthRepo(bankAccountId, _dir, _usr);


        private static string GetTransactionsDir(string mktDbFilePath)
        {
            var mktDir = Path.GetDirectoryName(mktDbFilePath);
            var txnDir = Path.Combine(mktDir, DIR_NAME);

            if (!Directory.Exists(txnDir))
                Directory.CreateDirectory(txnDir);

            return txnDir;
        }
    }
}
