using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.FileSystemTools;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.JsonTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.DailyColxnsRepository;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
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

        //private IJsonCache    _jCache = new TempJsonCache();
        private string        _foldrPath;
        private MarketStateDB _mkt;
        private ITenantDBsDir _dir;


        public CollectionsLocalDir(ITenantDBsDir tenantDBsDir)
        {
            _dir             = tenantDBsDir;
            _mkt             = _dir.MarketState;
            //_mkt             = marketStateDB;
            _foldrPath       = FindCollectionsDir(_mkt.DatabasePath);
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
            => Directory.EnumerateFiles (_foldrPath, "*.ldb")
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


        protected override ICollectionsDB ConnectToDB(DateTime date, string file)
        {
            var db                 = new SharedLiteDB(file, _mkt.CurrentUser);
            var colxnsDB           = new CollectionsDB1(date, db.Metadata, _mkt, file);
            SetIntendedColxns(colxnsDB.IntendedColxns, db);
            SetAmbulantColxns(colxnsDB.AmbulantColxns, db);
            SetUncollecteds  (colxnsDB.Uncollecteds  , db, date);
            SetNoOperations  (colxnsDB.NoOperations  , db);
            SetVacantStalls  (colxnsDB.VacantStalls  , db);
            colxnsDB.CashierColxns = new CashierColxnsRepo1(new CashierColxnsCollection(db));
            colxnsDB.OtherColxns   = new OtherColxnsRepo1(new OtherColxnsCollection(db));
            colxnsDB.BankDeposits  = new BankDepositsRepo1(new BankDepositsCollection(db));
            colxnsDB.BalanceAdjs   = new BalanceAdjsRepo1(date, new BalanceAdjsCollection(db), _mkt.Balances);
            return colxnsDB;
        }


        protected override bool TryFindDB(DateTime date, out string path)
        {
            path = AsFilePath(date);
            return File.Exists(path);
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


        private void SetUncollecteds(Dictionary<int, IUncollectedsRepo> dict, SharedLiteDB db, DateTime date)
        {
            foreach (var sec in _mkt.Sections.GetAll())
            {
                var colxn = new UncollectedsCollection(sec, db);
                var cache = GetSectionCache(sec);
                var repo  = new UncollectedsRepo1(cache, sec, date, colxn, _dir);
                dict.Add(sec.Id, repo);
            }
        }


        private IPersistentCollection<LeaseDTO> GetSectionCache(SectionDTO sec)
        {
            var fName = $"Section_{sec.Id}.ldb";
            var fPath = Path.Combine(Path.GetTempPath(), fName);
            return new LiteDBPersistentCollection<LeaseDTO>(fPath, "Inactives");
        }


        private void SetNoOperations(Dictionary<int, INoOperationsRepo> dict, SharedLiteDB db)
        {
            foreach (var sec in _mkt.Sections.GetAll())
            {
                var colxn = new NoOperationsCollection(sec, db);
                var repo  = new NoOperationsRepo1(colxn);
                dict.Add(sec.Id, repo);
            }
        }


        private void SetVacantStalls(Dictionary<int, IVacantStallsRepo> dict, SharedLiteDB db)
        {
            foreach (var sec in _mkt.Sections.GetAll())
            {
                var colxn = new VacantStallsCollection(sec, db);
                var repo  = new VacantStallsRepo1(colxn);
                dict.Add(sec.Id, repo);
            }
        }


        private string AsFilePath(DateTime date)
            => Path.Combine(_foldrPath, string.Format(FILENAME_FMT, date));
    }
}
