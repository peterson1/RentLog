using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DataSources;
using System.IO;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class InactivesLocalDir
    {
        public const string DIR_NAME      = "Inactives";
        public const string REQUESTS_FILE = "InactiveRequests.ldb";


        public static SharedLiteDB GetRequestsDB(ITenantDBsDir dir)
            => new SharedLiteDB(GetInactiveRequestsPath(dir), 
                                dir.MarketState.CurrentUser);


        private static string GetInactivesDir(ITenantDBsDir dir)
        {
            var mktDir  = Path.GetDirectoryName(dir.MarketState.DatabasePath);
            var dirPath = Path.Combine(mktDir, DIR_NAME);
            Directory.CreateDirectory(dirPath);
            return dirPath;
        }


        private static string GetInactiveRequestsPath(ITenantDBsDir dir)
        {
            var dbPath = Path.Combine(GetInactivesDir(dir), REQUESTS_FILE);

            //todo: uncomment this after AdHoc
            //if (!File.Exists(dbPath))
            //    throw Missing.File(dbPath, "Inactive Requests DB file");

            return dbPath;
        }
    }
}
