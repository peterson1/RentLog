using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using RentLog.DomainLib11.CollectionRepos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class CollectionsDBFolder : CollectionDBsBase
    {
        private const string COLLECTIONS_DIR = "Collections";

        private string _dir;
        private string _usr;


        public CollectionsDBFolder(string marketDbFilePath, string currentUser)
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
    }
}
