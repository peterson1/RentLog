using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.DailyBillsRepository;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using System.IO;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class BalancesLocalDir : BalanceDBBase
    {
        private const string BALANCES_DIR = "Balances";
        private const string FILENAME_FMT = "Lease{0:0000#}_Balance.ldb";

        private string _dir;
        private string _usr;


        public BalancesLocalDir(string marketDbFilePath, string currentUser)
        {
            _dir = FindBalancesDir(marketDbFilePath);
            _usr = currentUser;
        }


        public override IDailyBillsRepo GetRepo(LeaseDTO lse)
        {
            var file = Path.Combine(_dir, GetFilename(lse));
            var db   = new SharedLiteDB(file, _usr);
            return new DailyBillsRepo1(new DailyBillsCollection(db));
        }


        private static string GetFilename(LeaseDTO lease)
            => string.Format(FILENAME_FMT, lease.Id);


        private string FindBalancesDir(string marketDbFilePath)
        {
            var dbFile = marketDbFilePath.MakeAbsolute();
            var mktDir = Path.GetDirectoryName(dbFile);
            var balDir = Path.Combine(mktDir, BALANCES_DIR);
            if (!Directory.Exists(balDir))
                Directory.CreateDirectory(balDir);
            return balDir;
        }
    }
}
