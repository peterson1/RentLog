using CommonTools.Lib11.FileSystemTools;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib45.LiteDbTools
{
    public class LiteDBPersistentCollection<T> : IPersistentCollection<T>
    {
        private string  _filePath;
        private string  _colxnKey;
        private bool    _isAny;


        public LiteDBPersistentCollection(string filePath, string colxnKey)
        {
            _filePath = filePath;
            _colxnKey = colxnKey;
        }

        public bool Any() => _isAny;


        public IEnumerable<T> Enumerate()
        {
            using (var db = Connect(FileMode.ReadOnly))
            {
                var colxn = db.GetCollection<T>(_colxnKey);
                return colxn.FindAll();
            }
        }


        public void Clear()
        {
            using (var db = Connect(FileMode.Shared))
            {
                db.DropCollection(_colxnKey);
            }
            _isAny = false;
        }


        public void Set(IEnumerable<T> list)
        {
            using (var db = Connect(FileMode.Shared))
            {
                db.DropCollection(_colxnKey);
                var colxn = db.GetCollection<T>(_colxnKey);
                colxn.InsertBulk(list);
            }
            _isAny = true;
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
