using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.CollectorsRepository;
using RentLog.DatabaseLib.LeasesRepository;
using RentLog.DatabaseLib.SectionsRepository;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public static class MarketStateDBFile
    {
        private const string BRANCH_KEY = "BranchName";

        public static MarketStateDB Load(string marketDbFilePath, string currentUser)
        {
            var mkt          = new MarketStateDB();
            var db           = new SharedLiteDB(marketDbFilePath, currentUser);

            mkt.DatabasePath = marketDbFilePath;
            mkt.CurrentUser  = currentUser;
            mkt.BranchName   = db.Metadata[BRANCH_KEY];

            mkt.Stalls       = new StallsRepo1(new StallsCollection(db), mkt);
            mkt.Collectors   = new CollectorsRepo1(new CollectorsCollection(db), mkt);
            mkt.Sections     = new SectionsRepo1(new SectionsCollection(db), mkt);
            mkt.ActiveLeases = new ActiveLeasesRepo1(new ActiveLeasesCollection(db), mkt);

            return mkt;
        }
    }
}
