using CommonTools.Lib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib11.DatabaseTools
{
    public abstract class SimpleRepoShimBase<T> : ISimpleRepo<T>
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
        protected virtual void ExecuteAfterSave    (T record) { }
        protected virtual IOrderedEnumerable<T> ToSorted(IEnumerable<T> items) => items.OrderBy(_ => _.ToString());

        protected List<T> ToSortedList(IEnumerable<T> items) => ToSorted(items).ToList();


        public List<T>            GetAll       ()                                         => ToSortedList(_repo.GetAll());
        public T                  Find         (int recordId, bool errorIfMissing)        => _repo.Find(recordId, errorIfMissing);
        public bool               HasName      (string recordName, string field = "Name") => _repo.HasName(recordName, field);
        public Dictionary<int, T> ToDictionary ()                                         => _repo.ToDictionary();


        public int Insert(T newRecord)
        {
            ValidateBeforeInsert(newRecord);
            var id = _repo.Insert(newRecord);
            ExecuteAfterSave(newRecord);
            return id;
        }


        public bool Update(T changedRecord)
        {
            ValidateBeforeUpdate(changedRecord);
            var ok = _repo.Update(changedRecord);
            ExecuteAfterSave(changedRecord);
            return ok;
        }


        public bool Upsert(T record)
        {
            if (record is IDocumentDTO dto)
                return UpsertDTO(dto);
            else
            {
                var ok = _repo.Upsert(record);
                ExecuteAfterSave(record);
                return ok;
            }
        }

        private bool UpsertDTO(IDocumentDTO dto) => dto.Id == 0
                                ? Insert((T)dto) > 0 : Update((T)dto);


        public bool Delete(T record)
        {
            ValidateBeforeDelete(record);
            var ok = _repo.Delete(record);
            ExecuteAfterSave(record);
            return ok;
        }
    }
}
