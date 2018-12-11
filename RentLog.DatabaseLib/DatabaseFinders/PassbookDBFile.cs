using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.ChequeVouchersRepository;
using RentLog.DatabaseLib.FundRequestsRepository;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.MarketStateRepos;
using System.IO;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class PassbookDBFile : ChequeVouchersDB
    {
        public const string FILENAME = "Passbooks.ldb";
        private string _pbkDbPath;


        public PassbookDBFile(ITenantDBsDir dir)
        {
            _pbkDbPath       = GetDbPath(dir.MarketState);
            var pbkDb        = new SharedLiteDB(_pbkDbPath, dir.MarketState.CurrentUser);
            var inactvDb     = InactivesLocalDir.GetRequestsDB(dir);

            ActiveRequests   = new FundRequestsRepo1(new ActiveRequestsCollection(pbkDb));
            //InactiveRequests_old = new FundRequestsRepo1(new InactiveRequestsCollection(pbkDb));
            InactiveRequests = new FundRequestsRepo1(new InactiveRequestsCollection(inactvDb));
            PreparedCheques  = new PreparedChequesRepo1(new ChequeVouchersCollection(pbkDb));
            PassbookRows     = dir.Passbooks;
        }


        internal static string GetDbPath(MarketStateDbBase mkt)
        {
            var dir    = Path.GetDirectoryName(mkt.DatabasePath);
            var dbPath = Path.Combine(dir, FILENAME);

            //if (!File.Exists(dbPath))
            //    throw Missing.File(dbPath, "Passbook DB file");

            return dbPath;
        }
    }
}
