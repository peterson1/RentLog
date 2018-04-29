using CommonTools.Lib45.LiteDbTools;
using System.IO;

namespace RentLog.DatabaseLib
{
    public class RentLogDB : SharedLiteDB
    {
        public RentLogDB(string dbFilePath, string currentUser, bool doInitialize = true) : base(dbFilePath, currentUser, doInitialize)
        {
        }


        public RentLogDB(MemoryStream memoryStream, string currentUser, bool doInitialize = true) : base(memoryStream, currentUser, doInitialize)
        {
        }


        protected override void InitializeCollections()
        {
        }
    }
}
