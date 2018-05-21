using CommonTools.Lib11.CollectionTools;
using CommonTools.Lib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        protected virtual IEnumerable<T> ToSorted(IEnumerable<T> items) => items;

        protected List<T> ToSortedList(IEnumerable<T> items) => ToSorted(items).ToList();


        public List<T>            GetAll       ()                                         => ToSortedList(_repo.GetAll());
        public List<T>            Find         (Expression<Func<T, bool>> predicate)      => ToSortedList(_repo.Find(predicate));
        public T                  Find         (int recordId, bool errorIfMissing)        => _repo.Find(recordId, errorIfMissing);
        public T                  Earliest     () => _repo.Earliest();
        public T                  Latest       () => _repo.Latest();
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


        public void Update(IEnumerable<T> records, bool doValidate)
        {
            if (doValidate)
                records.ForEach(_ => ValidateBeforeUpdate(_));

            _repo.Update(records, doValidate);
        }
    }
}
