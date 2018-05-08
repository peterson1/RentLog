using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib11.DatabaseTools
{
    public abstract class SimpleRepoShimBase<T>
    {
        public event EventHandler<T> ContentChanged = delegate { };

        protected ISimpleRepo<T>  _repo;


        public SimpleRepoShimBase(ISimpleRepo<T> simpleRepo)
        {
            _repo = simpleRepo;
            _repo.ContentChanged += (s, e) => ContentChanged?.Invoke(s, e);
        }


        protected virtual void ValidateBeforeInsert(T newRecord) { }
        protected virtual void ValidateBeforeUpdate(T changedRecord) { }
        protected virtual void ValidateBeforeDelete(T record) { }
        protected virtual IOrderedEnumerable<T> ToSorted(IEnumerable<T> items) => items.OrderBy(_ => _.ToString());

        protected List<T> ToSortedList(IEnumerable<T> items) => ToSorted(items).ToList();


        public List<T> GetAll() => ToSortedList(_repo.GetAll());


        public int Insert(T newRecord)
        {
            ValidateBeforeInsert(newRecord);
            return _repo.Insert(newRecord);
        }


        public bool Update(T changedRecord)
        {
            ValidateBeforeUpdate(changedRecord);
            return _repo.Update(changedRecord);
        }


        public bool Delete(T record)
        {
            ValidateBeforeDelete(record);
            return _repo.Delete(record);
        }
    }
}
