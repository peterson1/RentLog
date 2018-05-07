using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib45.LiteDbTools
{
    public abstract class SharedCollectionShim<T> : ISimpleRepo<T>
        where T : IDocumentDTO
    {
        public event EventHandler<T> ContentChanged;

        protected SharedCollectionBase<T> _colxn;


        public SharedCollectionShim(SharedLiteDB sharedLiteDB)
        {
            _colxn = GetSharedCollection(sharedLiteDB);
            _colxn.ContentChanged += (s, e) => this.ContentChanged?.Invoke(s, e);
        }


        protected abstract SharedCollectionBase<T> GetSharedCollection (SharedLiteDB sharedLiteDB);
        public abstract IEnumerable<T> ToSorted(IEnumerable<T> items);


        public List<T> ToSortedList(IEnumerable<T> items) => ToSorted(items).ToList();


        public virtual List<T> GetAll ()                => _colxn.GetAll();
        public virtual bool    Delete (T record)        => _colxn.Delete(record);
        public virtual bool    Update (T changedRecord) => _colxn.Update(changedRecord);


        public virtual int Insert(T newRecord)
        {
            BeforeInsert(newRecord);
            return _colxn.Insert(newRecord);
        }

        protected virtual void BeforeInsert(T newRecord) { }
    }
}
