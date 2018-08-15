using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CommonTools.Lib11.DatabaseTools
{
    public class CombinedReadOnlyRepo<T> : ISimpleRepo<T>
    {
        private ISimpleRepo<T> _list1;
        private ISimpleRepo<T> _list2;

        public CombinedReadOnlyRepo(ISimpleRepo<T> list1, ISimpleRepo<T> list2)
        {
            _list1 = list1;
            _list2 = list2;
        }


        public List<T> GetAll() 
            => _list1.GetAll().Concat(_list2.GetAll()).ToList();


        public event EventHandler<T> ContentChanged = delegate { };
        public bool Any() => throw new NotImplementedException();
        public bool Delete(int recordId) => throw new NotImplementedException();
        public bool Delete(List<T> records) => throw new NotImplementedException();
        public bool Delete(T unwantedRecord) => throw new NotImplementedException();
        public void Drop() => throw new NotImplementedException();
        public void DropAndInsert(IEnumerable<T> records, bool doValidate) => throw new NotImplementedException();
        public T Earliest() => throw new NotImplementedException();
        public T Find(int recordId, bool errorIfMissing) => throw new NotImplementedException();
        public List<T> Find(Expression<Func<T, bool>> predicate) => throw new NotImplementedException();
        public bool HasId(int recordId) => throw new NotImplementedException();
        public bool HasName(string recordName, string field = "Name") => throw new NotImplementedException();
        public void Insert(IEnumerable<T> records, bool doValidate) => throw new NotImplementedException();
        public int Insert(T newRecord) => throw new NotImplementedException();
        public bool IsValidForDelete(T record, out string whyInvalid) => throw new NotImplementedException();
        public bool IsValidForInsert(T draft, out string whyInvalid) => throw new NotImplementedException();
        public bool IsValidForUpdate(T record, out string whyInvalid) => throw new NotImplementedException();
        public T Latest() => throw new NotImplementedException();
        public int Max(Expression<Func<T, int>> getter) => throw new NotImplementedException();
        public DateTime Max(Expression<Func<T, DateTime>> getter) => throw new NotImplementedException();
        public bool TableExists() => throw new NotImplementedException();
        public Dictionary<int, T> ToDictionary() => throw new NotImplementedException();
        public void Update(IEnumerable<T> records, bool doValidate) => throw new NotImplementedException();
        public bool Update(T changedRecord) => throw new NotImplementedException();
        public void Upsert(IEnumerable<T> records, bool doValidate) => throw new NotImplementedException();
        public bool Upsert(T record) => throw new NotImplementedException();
    }
}
