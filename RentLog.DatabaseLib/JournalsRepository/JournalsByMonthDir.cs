using System;
using System.Collections.Generic;
using System.IO;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.JournalVoucherRepos;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.DatabaseLib.JournalsRepository
{
    public class JournalsByMonthDir : ShardedJournalsRepoBase
    {
        private const string DIR_NAME = "Journals";
        private const string NAME_FMT = "{0:yyyy-MM}_JVs_Shard.ldb";

        private string _foldr;
        private string _usr;


        public JournalsByMonthDir(MarketStateDB marketStateDB)
        {
            _usr   = marketStateDB.CurrentUser;
            _foldr = FindJournalsFolder(marketStateDB.DatabasePath);
        }


        private string FindJournalsFolder(string mktDbFilePath)
        {
            var mktDir = Path.GetDirectoryName(mktDbFilePath);
            var jorDir = Path.Combine(mktDir, DIR_NAME);

            if (!Directory.Exists(jorDir))
                Directory.CreateDirectory(jorDir);

            return jorDir;
        }


        protected override ISimpleRepo<JournalVoucherDTO> ConnectToDB(string databasePath)
        {
            var db  = new SharedLiteDB(databasePath, _usr);
            var col = new JournalVouchersCollection(db);
            return new JournalSoloShard1(col);
        }


        protected override string GetDatabasePath(DateTime date)
            => Path.Combine(_foldr, string.Format(NAME_FMT, date));


        protected override List<string> GetAllDatabasePaths()
        {
            throw new NotImplementedException();
        }
    }
}
