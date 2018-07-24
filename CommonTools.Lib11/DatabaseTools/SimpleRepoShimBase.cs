using CommonTools.Lib11.CollectionTools;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.StringTools;
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


        protected virtual void   ValidateBeforeInsert(T newRecord) { }
        protected virtual void   ValidateBeforeUpdate(T changedRecord) { }
        protected virtual void   ValidateBeforeDelete(T record) { }
        protected virtual void   ExecuteAfterSave    (T record, bool operationIsDelete) { }
        protected virtual string GetWhyInvalid       (T dto) => string.Empty;
        protected virtual IEnumerable<T> ToSorted(IEnumerable<T> items) => items;

        protected List<T> ToSortedList(IEnumerable<T> items) => ToSorted(items).ToList();


        public bool               Any           ()                                         => _repo.Any();
        public List<T>            GetAll        ()                                         => ToSortedList(_repo.GetAll());
        public List<T>            Find          (Expression<Func<T, bool>> predicate)      => ToSortedList(_repo.Find(predicate));
        public T                  Find          (int recordId, bool errorIfMissing)        => _repo.Find(recordId, errorIfMissing);
        public T                  Earliest      ()                                         => _repo.Earliest();
        public T                  Latest        ()                                         => _repo.Latest();
        public int                Max           (Expression<Func<T, int>> getter)          => _repo.Max(getter);
        public DateTime           Max           (Expression<Func<T, DateTime>> getter)     => _repo.Max(getter);
        public bool               HasId         (int recordId)                             => _repo.HasId(recordId);
        public bool               HasName       (string recordName, string field = "Name") => _repo.HasName(recordName, field);
        public void               Drop          ()                                         => _repo.Drop();
        public void               DropAndInsert (IEnumerable<T> records, bool doValidate)  => _repo.DropAndInsert(records, doValidate);
        public Dictionary<int, T> ToDictionary  ()                                         => _repo.ToDictionary();
        public bool               TableExists   ()                                         => _repo.TableExists();


        public int Insert(T newRecord)
        {
            ValidateBeforeInsert(newRecord);
            var id = _repo.Insert(newRecord);
            ExecuteAfterSave(newRecord, false);
            return id;
        }


        public bool Update(T changedRecord)
        {
            ValidateBeforeUpdate(changedRecord);
            var ok = _repo.Update(changedRecord);
            if (!ok) return false;
            ExecuteAfterSave(changedRecord, false);
            return ok;
        }


        public bool Upsert(T record)
        {
            if (record is IDocumentDTO dto)
                return UpsertDTO(dto);
            else
            {
                var ok = _repo.Upsert(record);
                ExecuteAfterSave(record, false);
                return ok;
            }
        }

        private bool UpsertDTO(IDocumentDTO dto) => dto.Id == 0
                                ? Insert((T)dto) > 0 : Update((T)dto);


        public bool Delete(T record)
        {
            ValidateBeforeDelete(record);
            var ok = _repo.Delete(record);
            ExecuteAfterSave(record, true);
            return ok;
        }


        public bool Delete(List<T> records)
        {
            if (records == null || !records.Any()) return false;

            foreach (var rec in records)
                ValidateBeforeDelete(rec);

            var ok = _repo.Delete(records);

            ExecuteAfterSave(records.Last(), true);
            return ok;
        }


        public bool Delete(int recordId) => _repo.Delete(recordId);


        public void Insert(IEnumerable<T> records, bool doValidate)
        {
            if (records == null || !records.Any()) return;

            if (doValidate)
                records.ForEach(_ => ValidateBeforeInsert(_));

            _repo.Insert(records, doValidate);
        }


        public void Update(IEnumerable<T> records, bool doValidate)
        {
            if (records == null || !records.Any()) return;

            if (doValidate)
                records.ForEach(_ => ValidateBeforeUpdate(_));

            _repo.Update(records, doValidate);
        }


        public void Upsert(IEnumerable<T> records, bool doValidate)
        {
            if (records == null || !records.Any()) return;

            if (doValidate)
                records.ForEach(_ => ValidateBeforeUpdate(_));

            _repo.Upsert(records, doValidate);
        }


        public virtual bool IsValidForInsert(T draft, out string whyInvalid)
        {
            if (!IsIdZero(draft, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(draft);
            return whyInvalid.IsBlank();
        }


        public virtual bool IsValidForUpdate(T record, out string whyInvalid)
        {
            if (!HasValidRecordId(record, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(record);
            return whyInvalid.IsBlank();
        }

        public virtual bool IsValidForDelete(T record, out string whyInvalid)
        {
            if (!HasValidRecordId(record, out whyInvalid)) return false;
            whyInvalid = string.Empty;
            return true;
        }


        private bool IsIdZero(T draft, out string whyInvalid)
        {
            if (!(draft is IDocumentDTO doc))
            {
                whyInvalid = string.Empty;
                return true;
            }
            whyInvalid = doc.Id == 0 ? string.Empty
                       : $"‹{typeof(T).Name}› Id for insert should be zero, but was [{doc.Id}].";

            return whyInvalid.IsBlank();
        }


        private bool HasValidRecordId(T record, out string whyInvalid)
        {
            if (!(record is IDocumentDTO doc))
            {
                whyInvalid = string.Empty;
                return true;
            }
            whyInvalid = doc.Id > 0 ? string.Empty
                       : $"‹{typeof(T).Name}› Id should be greater than zero, but was [{doc.Id}].";

            return whyInvalid.IsBlank();
        }
    }
}
