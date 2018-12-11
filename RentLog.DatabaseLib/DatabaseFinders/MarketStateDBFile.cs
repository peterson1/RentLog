using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.BankAccountsRepository;
using RentLog.DatabaseLib.CollectorsRepository;
using RentLog.DatabaseLib.GLAccountsRepository;
using RentLog.DatabaseLib.LeasesRepository;
using RentLog.DatabaseLib.SectionsRepository;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.MarketStateRepos;
using System;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class MarketStateDBFile : MarketStateDbBase
    {
        private const string SYSTEM_NAME_KEY = "SystemName";
        private const string BRANCH_NAME_KEY = "BranchName";
        private const string YEARS_BACK_KEY  = "YearsBack";
        private string      _mktDbPath;
        private string      _currUsr;
        private string      _systemName;
        private string      _branchName;
        private int         _yrsBackCount;


        public MarketStateDBFile(string marketDbFilePath, string currentUser)
        {
            _mktDbPath     = marketDbFilePath;
            _currUsr       = currentUser;
            DatabasePath   = marketDbFilePath;
            CurrentUser    = currentUser;
            var mktDb      = new SharedLiteDB(_mktDbPath, _currUsr);
            Stalls         = new StallsRepo1        (new StallsCollection        (mktDb), this);
            Collectors     = new CollectorsRepo1    (new CollectorsCollection    (mktDb), this);
            BankAccounts   = new BankAccountsRepo1  (new BankAccountsCollection  (mktDb), this);
            Sections       = new SectionsRepo1      (new SectionsCollection      (mktDb), this);
            ActiveLeases   = new ActiveLeasesRepo1  (new ActiveLeasesCollection  (mktDb), this);
            InactiveLeases = new InactiveLeasesRepo1(new InactiveLeasesCollection(mktDb), this);
            CacheMetadataFields(mktDb);
        }


        private void CacheMetadataFields(SharedLiteDB mktDb)
        {
            _systemName   = mktDb.Metadata[SYSTEM_NAME_KEY];
            _branchName   = mktDb.Metadata[BRANCH_NAME_KEY];
            _yrsBackCount = int.TryParse(mktDb.Metadata[YEARS_BACK_KEY], out int num) ? num : 2;
        }


        public override IGLAccountsRepo GLAccounts
        {
            get => base.GLAccounts ?? TryLoadPassbookDB(_ => _.GLAccounts);
            set => base.GLAccounts = value;
        }

        public override string SystemName
        {
            get => _systemName;
            set => MktDB.Metadata[SYSTEM_NAME_KEY] = _systemName = value;
        }

        public override string BranchName
        {
            get => _branchName;
            set => MktDB.Metadata[BRANCH_NAME_KEY] = _branchName = value;
        }


        private T TryLoadPassbookDB<T>(Func<MarketStateDbBase, T> getter)
        {
            //var dir    = Path.GetDirectoryName(_mktDbPath);
            //var dbPath = Path.Combine(dir, PassbookDBFile.FILENAME);
            //
            //if (!File.Exists(dbPath))
            //    throw Missing.File(dbPath, "Passbook DB file");

            var dbPath = PassbookDBFile.GetDbPath(this);
            var pbkDb  = new SharedLiteDB(dbPath, _currUsr);
            GLAccounts = new GLAccountsRepo1(new GLAccountsCollection(pbkDb), this);
            return getter(this);
        }


        public override int YearsBackCount => _yrsBackCount;
        private SharedLiteDB MktDB => new SharedLiteDB(_mktDbPath, _currUsr);
    }
}
