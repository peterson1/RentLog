using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.LiteDbTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.Repositories
{
    //core candidate
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
        protected abstract IEnumerable<T> ToSorted(IEnumerable<T> items);


        protected List<T> ToSortedList(IEnumerable<T> items) => ToSorted(items).ToList();


        public virtual List<T> GetAll ()                => _colxn.GetAll();
        public virtual int     Insert (T newRecord)     => _colxn.Insert(newRecord);
        public virtual bool    Delete (T record)        => _colxn.Delete(record);
        public virtual bool    Update (T changedRecord) => _colxn.Update(changedRecord);
    }
}
