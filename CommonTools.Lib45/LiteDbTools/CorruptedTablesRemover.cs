using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib45.FileSystemTools;
using LiteDB;

namespace CommonTools.Lib45.LiteDbTools
{
    public static class CorruptedTablesRemover
    {
        public static void RemoveCorruptedTables(this SharedLiteDB sharedLiteDB)
        {
            var tmp    = TempFile.NewNonExistentPath();
            var orig   = sharedLiteDB.DbPath;
            var tables = GetTableNames(orig);

            foreach (var tblName in tables)
                CopyTable(tblName, orig, tmp);

            orig.DeleteIfFound();
            System.IO.File.Move(tmp, orig);
        }


        private static List<string> GetTableNames(string dbPath)
        {
            using (var db = NewLiteDB(dbPath, FileMode.ReadOnly))
                return db.GetCollectionNames().ToList();
        }


        private static void CopyTable(string tblName, string sourcePath, string destinationPath)
        {
            var all = GetAllRecords(sourcePath, tblName);
            if (all == null) return;

            using (var db = NewLiteDB(destinationPath, FileMode.Shared))
                db.GetCollection(tblName).InsertBulk(all);
        }


        private static List<BsonDocument> GetAllRecords(string dbPath, string tblName)
        {
            try
            {
                using (var db = NewLiteDB(dbPath, FileMode.ReadOnly))
                    return db.GetCollection(tblName).FindAll().ToList();
            }
            catch  { return null; }
        }


        private static LiteDatabase NewLiteDB(string dbPath, FileMode fileMode) => new LiteDatabase(new ConnectionString
        {
            Filename  = dbPath,
            Journal   = false,
            Mode      = fileMode,
            LimitSize = long.MaxValue
        });
    }
}
