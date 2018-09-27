using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LiteDbTools;
using FluentAssertions;
using LiteDB;
using System.Linq;
using Xunit;

namespace CommonTools.Tests.LiteDbToolsTests
{
    [Trait("LiteDbTools", "TempCopy")]
    public class CorruptedTableRemoverFacts
    {
        [Fact(DisplayName = "Not watching for changes")]
        public void TestMethod00001()
        {
            var dbPath = TemmpCopyOf.FMEY_20180924();
            var tmpDb  = new SharedLiteDB(dbPath, "Test User", false);

            tmpDb.RemoveCorruptedTables();

            TableCount(dbPath).Should().Be(14);

            //await Task.Delay(1000 * 2);
            dbPath.DeleteIfFound();
        }


        private int TableCount(string dbPath)
        {
            using (var db = GetLiteDB(dbPath))
                return db.GetCollectionNames().Count();
        }


        private LiteDatabase GetLiteDB(string dbPath) => new LiteDatabase(new ConnectionString
        {
            Filename  = dbPath,
            Journal   = false,
            Mode      = LiteDB.FileMode.ReadOnly,
            LimitSize = long.MaxValue
        });
    }
}
