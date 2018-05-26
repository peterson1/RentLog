using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.DailyColxnsRepository;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class CollectionsLocalDir : CollectionsDirBase
    {
        private const string COLLECTIONS_DIR = "Collections";
        private const string FILENAME_FMT    = "{0:yyyy-MM-dd}_Solo.ldb";

        private string        _dir;
        private MarketStateDB _mkt;


        public CollectionsLocalDir(MarketStateDB marketStateDB)
        {
            _mkt             = marketStateDB;
            _dir             = FindCollectionsDir(_mkt.DatabasePath);
            _mkt.Collections = this;
        }


        private static string FindCollectionsDir(string marketDbFilePath)
        {
            var dbFile = marketDbFilePath.MakeAbsolute();
            var mktDir = Path.GetDirectoryName(dbFile);
            var dayDir = Path.Combine(mktDir, COLLECTIONS_DIR);
            if (!Directory.Exists(dayDir))
                Directory.CreateDirectory(dayDir);
            return dayDir;
        }


        protected override IEnumerable<DateTime> FindAllDates()
            => Directory.EnumerateFiles (_dir, "*.ldb")
                        .Select         (_ => AsDate(_));


        private static DateTime AsDate(string fileName)
        {
            var fName = Path.GetFileNameWithoutExtension(fileName);
            var dateStr = fName.Before("_");

            if (DateTime.TryParse(dateStr, out DateTime date))
                return date;
            else
                throw Fault.BadArg("date prefix", dateStr);
        }


        public override ICollectionsDB For (DateTime date)
        {
            var file               = AsFilePath(date);
            var db                 = new SharedLiteDB(file, _mkt.CurrentUser);
            var colxnsDB           = new CollectionsDB1(db.Metadata, _mkt);
            SetIntendedColxns(colxnsDB.IntendedColxns, db);
            SetAmbulantColxns(colxnsDB.AmbulantColxns, db);
            SetUncollecteds  (colxnsDB.Uncollecteds  , db);
            colxnsDB.CashierColxns = new CashierColxnsRepo1(new CashierColxnsCollection(db));
            colxnsDB.OtherColxns   = new OtherColxnsRepo1(new OtherColxnsCollection(db));
            colxnsDB.BankDeposits  = new BankDepositsRepo1(new BankDepositsCollection(db));
            colxnsDB.BalanceAdjs   = new BalanceAdjsRepo1(date, new BalanceAdjsCollection(db), _mkt.Balances);
            return colxnsDB;
        }


        private void SetIntendedColxns(Dictionary<int, IIntendedColxnsRepo> dict, SharedLiteDB db)
        {
            foreach (var sec in _mkt.Sections.GetAll())
            {
                var colxn = new IntendedColxnsCollection(sec, db);
                var repo  = new IntendedColxnsRepo1(colxn);
                dict.Add(sec.Id, repo);
            }
        }


        private void SetAmbulantColxns(Dictionary<int, IAmbulantColxnsRepo> dict, SharedLiteDB db)
        {
            foreach (var sec in _mkt.Sections.GetAll())
            {
                var colxn = new AmbulantColxnsCollection(sec, db);
                var repo  = new AmbulantColxnsRepo1(colxn);
                dict.Add(sec.Id, repo);
            }
        }


        private void SetUncollecteds(Dictionary<int, IUncollectedsRepo> dict, SharedLiteDB db)
        {
            foreach (var sec in _mkt.Sections.GetAll())
            {
                var colxn = new UncollectedsCollection(sec, db);
                var repo  = new UncollectedsRepo1(colxn);
                dict.Add(sec.Id, repo);
            }
        }


        private string AsFilePath(DateTime date)
            => Path.Combine(_dir, string.Format(FILENAME_FMT, date));
    }
}
