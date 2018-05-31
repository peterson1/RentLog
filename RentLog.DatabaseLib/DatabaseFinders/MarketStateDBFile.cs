using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.CollectorsRepository;
using RentLog.DatabaseLib.GLAccountsRepository;
using RentLog.DatabaseLib.LeasesRepository;
using RentLog.DatabaseLib.SectionsRepository;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.MarketStateRepos;
using System.IO;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public static class MarketStateDBFile
    {
        private const string SYSTEM_NAME_KEY = "SystemName";
        private const string BRANCH_NAME_KEY = "BranchName";
        private const string PASSBOOKDB_FILE = "Passbooks.ldb";


        public static MarketStateDB Load(string marketDbFilePath, string currentUser)
        {
            var mkt            = new MarketStateDB();
            var mktDb          = new SharedLiteDB(marketDbFilePath, currentUser);
            var pbkDb          = new SharedLiteDB(FindPassbookDB(marketDbFilePath), currentUser);
                               
            mkt.DatabasePath   = marketDbFilePath;
            mkt.CurrentUser    = currentUser;
            mkt.SystemName     = mktDb.Metadata[SYSTEM_NAME_KEY];
            mkt.BranchName     = mktDb.Metadata[BRANCH_NAME_KEY];
                               
            mkt.Stalls         = new StallsRepo1(new StallsCollection(mktDb), mkt);
            mkt.Collectors     = new CollectorsRepo1(new CollectorsCollection(mktDb), mkt);
            mkt.Sections       = new SectionsRepo1(new SectionsCollection(mktDb), mkt);
            mkt.ActiveLeases   = new ActiveLeasesRepo1(new ActiveLeasesCollection(mktDb), mkt);
            mkt.InactiveLeases = new InactiveLeasesRepo1(new InactiveLeasesCollection(mktDb), mkt);
            mkt.GLAccounts     = new GLAccountsRepo1(new GLAccountsCollection(pbkDb), mkt);

            return mkt;
        }


        private static string FindPassbookDB(string marketDbFilePath)
        {
            var dir  = Path.GetDirectoryName(marketDbFilePath);
            var path = Path.Combine(dir, PASSBOOKDB_FILE);

            if (!File.Exists(path))
                throw Missing.File(path, "Passbook DB file");

            return path;
        }
    }
}
