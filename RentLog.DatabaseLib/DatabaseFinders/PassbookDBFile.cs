using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.ChequeVouchersRepository;
using RentLog.DatabaseLib.FundRequestsRepository;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.MarketStateRepos;
using System.IO;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class PassbookDBFile : ChequeVouchersDB
    {
        private const string FILENAME = "Passbooks.ldb";
        private MarketStateDB _mkt;
        private string        _pbkDbPath;


        public PassbookDBFile(MarketStateDB marketStateDB)
        {
            _mkt             = marketStateDB;
            _pbkDbPath       = GetDbPath(marketStateDB);
            var pbkDb        = new SharedLiteDB(_pbkDbPath, _mkt.CurrentUser);

            ActiveRequests   = new FundRequestsRepo1(new ActiveRequestsCollection(pbkDb));
            InactiveRequests = new FundRequestsRepo1(new InactiveRequestsCollection(pbkDb));
            PreparedCheques  = new PreparedChequesRepo1(new ChequeVouchersCollection(pbkDb));
            IssuedCheques    = new IssuedChequesRepo1(new ChequeVouchersCollection(pbkDb));
        }


        internal static string GetDbPath(MarketStateDB mkt)
        {
            var dir    = Path.GetDirectoryName(mkt.DatabasePath);
            var dbPath = Path.Combine(dir, FILENAME);

            if (!File.Exists(dbPath))
                throw Missing.File(dbPath, "Passbook DB file");

            return dbPath;
        }
    }
}
