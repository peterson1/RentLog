using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.DailyBillsRepository;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.IO;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class BalancesLocalDir : BalanceDBBase
    {
        private const string BALANCES_DIR = "Balances";
        private const string FILENAME_FMT = "Lease{0:0000#}_Balance.ldb";

        private string        _foldrPath;
        private MarketStateDbBase _mkt;
        private ITenantDBsDir _dir;


        public BalancesLocalDir(ITenantDBsDir tenantDBsDir)
        {
            _dir = tenantDBsDir;
            _mkt = _dir.MarketState;
            //_mkt = marketStateDB;
            _foldrPath = FindBalancesDir(_mkt.DatabasePath);
            _mkt.Balances = this;
        }


        public override IDailyBillsRepo GetRepo(int lseID)
        {
            var file = Path.Combine(_foldrPath, GetFilename(lseID));
            var db   = new SharedLiteDB(file, _mkt.CurrentUser);
            var lse  = _mkt.FindLease(lseID);
            return new DailyBillsRepo1(lse, new DailyBillsCollection(db), _dir);
        }


        public override IDailyBillsRepo GetRepo(LeaseDTO lse)
        {
            var file = Path.Combine(_foldrPath, GetFilename(lse.Id));
            var db   = new SharedLiteDB(file, _mkt.CurrentUser);
            return new DailyBillsRepo1(lse, new DailyBillsCollection(db), _dir);
        }


        private static string GetFilename(int lseID)
            => string.Format(FILENAME_FMT, lseID);


        private string FindBalancesDir(string marketDbFilePath)
        {
            var dbFile = marketDbFilePath.MakeAbsolute();
            var mktDir = Path.GetDirectoryName(dbFile);
            var balDir = Path.Combine(mktDir, BALANCES_DIR);
            if (!Directory.Exists(balDir))
                Directory.CreateDirectory(balDir);
            return balDir;
        }

        protected override MarketStateDbBase GetMarketState() => _mkt;
    }
}
