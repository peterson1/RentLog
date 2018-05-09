using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.DailyColxnsRepository;
using RentLog.DomainLib11.CollectionRepos;
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

        private string _dir;
        private string _usr;


        public CollectionsLocalDir(string marketDbFilePath, string currentUser)
        {
            _dir = FindCollectionsDir(marketDbFilePath);
            _usr = currentUser;
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


        protected override ICollectionsDB GetDB(DateTime date)
        {
            var file          = AsFilePath(date);
            var db            = new SharedLiteDB(file, _usr);
            var cashierColxns = new CashierColxnsRepo1(new CashierColxnsCollection(db));
            return new CollectionsDB1(db.Metadata, cashierColxns);
        }


        private string AsFilePath(DateTime date)
            => Path.Combine(_dir, string.Format(FILENAME_FMT, date));
    }
}
