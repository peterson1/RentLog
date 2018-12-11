using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.JournalVoucherRepos;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.DatabaseLib.JournalsRepository
{
    public class JournalsByMonthDir : ShardedJournalsRepoBase
    {
        private const string DIR_NAME  = "Journals";
        private const string JV_SUFFIX = "_JVs_Shard.ldb";
        private const string NAME_FMT  = "{0:yyyy-MM}" + JV_SUFFIX;

        private string _foldr;
        private string _usr;


        public JournalsByMonthDir(MarketStateDbBase marketStateDB)
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
            => Directory.GetFiles(_foldr, $"*{JV_SUFFIX}").ToList();


        protected override async Task<List<int>> GetParallelResults(List<Func<int>> jobs)
        {
            var bag = new ConcurrentBag<int>();

            await Task.Run(() 
                => Parallel.ForEach(jobs, job 
                    => bag.Add(job.Invoke())));

            return bag.ToList();
        }


        //private void SomeMethod()
        //{
        //    Parallel.ForEach()
        //}
    }
}
