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
        private string _pbkDbPath;


        public TransactionsByMonthDir(MarketStateDB marketStateDB)
        {
            _dir       = GetTransactionsDir(marketStateDB.DatabasePath);
            _usr       = marketStateDB.CurrentUser;
            _pbkDbPath = GetPassbookDbPath(marketStateDB.DatabasePath);
        }


        private string GetPassbookDbPath(string marketStateDbPath)
            => Path.Combine(Path.GetDirectoryName(marketStateDbPath),
                            PassbookDBFile.FILENAME);


        public IPassbookRowsRepo GetRepo(int bankAccountId)
        {
            var pbkDB = new SharedLiteDB(_pbkDbPath, _usr);
            var meta  = pbkDB.Metadata;
            return new TransactionsByMonthRepo(bankAccountId, _dir, _usr, meta);
        }

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
