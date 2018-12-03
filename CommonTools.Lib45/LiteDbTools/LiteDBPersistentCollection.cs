using CommonTools.Lib11.FileSystemTools;
using LiteDB;
using System.Collections.Generic;

namespace CommonTools.Lib45.LiteDbTools
{
    public class LiteDBPersistentCollection<T> : IPersistentCollection<T>
    {
        private string            _filePath;
        private string            _colxnKey;
        private LiteDatabase      _db;
        private LiteCollection<T> _colxn;


        public LiteDBPersistentCollection(string filePath, string colxnKey)
        {
            _filePath = filePath;
            _colxnKey = colxnKey;
            StandbyAs(FileMode.ReadOnly);
        }


        public bool           Any       () => _colxn.Count() != 0;
        public IEnumerable<T> Enumerate () => _colxn.FindAll();


        public void Clear()
        {
            Disconnect();
            using (var db = Connect(FileMode.Shared))
            {
                db.DropCollection(_colxnKey);
            }
            StandbyAs(FileMode.ReadOnly);
        }


        public void Set(IEnumerable<T> list)
        {
            Disconnect();
            using (var db = Connect(FileMode.Shared))
            {
                db.DropCollection(_colxnKey);
                var colxn = db.GetCollection<T>(_colxnKey);
                colxn.InsertBulk(list);
            }
            StandbyAs(FileMode.ReadOnly);
        }


        private void Disconnect()
        {
            _colxn = null;
            _db?.Dispose();
            _db = null;
        }


        private void StandbyAs(FileMode fileMode)
        {
            _db    = Connect(fileMode);
            _colxn = _db.GetCollection<T>(_colxnKey);
        }


        private LiteDatabase Connect(FileMode fileMode) => new LiteDatabase(new ConnectionString
        {
            Filename  = _filePath,
            Journal   = false,
            Mode      = fileMode,
            LimitSize = long.MaxValue
        });
    }
}
