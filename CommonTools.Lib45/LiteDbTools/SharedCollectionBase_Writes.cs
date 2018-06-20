﻿using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommonTools.Lib45.LiteDbTools
{
    public abstract partial class SharedCollectionBase<T>
        where T : IDocumentDTO
    {
        public event EventHandler<T> ContentChanged;
        private bool _hasCustomIndeces = true;


        protected virtual void EnsureIndeces(LiteCollection<T> coll)
        {
            _hasCustomIndeces = false;
        }


        protected virtual void Validate(T model, SharedLiteDB db)
        {
        }


        public int Insert(T record)
            => Write(record, (c, r) => c.Insert(r));


        public bool Update(T record)
            => Write(record, (c, r) => c.Update(r));


        public bool Upsert(T record) 
            => Write(record, (c, r) => c.Upsert(r));


        public void Insert(IEnumerable<T> records, bool doValidate = true)
            => WriteBulk(records, (c, r) => c.InsertBulk(r), doValidate);


        public void Update(IEnumerable<T> records, bool doValidate = true)
            => WriteBulk(records, (c, r) => c.Update(r), doValidate);


        public int Upsert(IEnumerable<T> records, bool doValidate = true)
            => WriteBulk(records, (c, r) => c.Upsert(r), doValidate);


        public bool Delete(int recordId)
        {
            using (var db = _db.OpenWrite())
                return GetCollection(db).Delete(recordId);
        }


        public bool Delete(T record)
        {
            var ok = false;
            using (var db = _db.OpenWrite())
            {
                var coll = GetCollection(db);
                ok = coll.Delete(record.Id);
            }
            if (ok) ContentChanged?.Invoke(this, record);
            return ok;
        }


        public int Delete(Expression<Func<T, bool>> predicate)
        {
            var deletd = 0;
            using (var db = _db.OpenWrite())
            {
                var coll = GetCollection(db);
                deletd   = coll.Delete(predicate);
            }
            return deletd;
        }


        private TOut Write<TOut>(T record, 
            Func<LiteCollection<T>, T, TOut> func)
        {
            SetCurrentFields(record);
            Validate(record, _db);
            TOut ret;
            using (var db = _db.OpenWrite())
            {
                var coll = GetCollection(db);
                EnsureIndeces(coll);
                ret = func(coll, record);
            }
            ContentChanged?.Invoke(this, record);
            return ret;
        }


        private int WriteBulk(IEnumerable<T> records,
            Func<LiteCollection<T>, IEnumerable<T>, int> func,
            bool doValidate)
        {
            foreach (var model in records)
            {
                SetCurrentFields(model);
                if (doValidate) Validate(model, _db);
            }

            var ret = 0;
            using (var db = _db.OpenWrite())
            {
                var coll = GetCollection(db);
                EnsureIndeces(coll);
                ret = func(coll, records);
            }
            ContentChanged?.Invoke(this, default(T));
            return ret;
        }


        //private void ReplaceAll_dep(IEnumerable<T> newRecords, bool doValidate = true, bool clearIDs = false)
        //{
        //    foreach (var model in newRecords)
        //    {
        //        SetCurrentFields(model);
        //        if (clearIDs) model.Id = 0;
        //        if (doValidate) Validate(model, _db);
        //    }

        //    string colxnName = "";
        //    using (var db = _db.OpenRead())
        //        colxnName = GetCollection(db).Name;

        //    using (var db = _db.OpenWrite())
        //        db.DropCollection(colxnName);

        //    using (var db = _db.OpenWrite())
        //    {
        //        var coll = GetCollection(db);
        //        EnsureIndeces(coll);
        //        coll.InsertBulk(newRecords);
        //    }
        //    ContentChanged?.Invoke(this, default(T));
        //}


        public void DropAndInsert(IEnumerable<T> records, bool doValidate)
        {
            foreach (var model in records)
            {
                SetCurrentFields(model);
                if (doValidate) Validate(model, _db);
            }
            Drop();
            Insert(records, false);
        }


        public void Drop()
        {
            using (var db = _db.OpenWrite())
                db.DropCollection(GetCollection(db).Name);
        }


        protected void SetCurrentFields(IDocumentDTO model)
        {
            model.Author    = _db.CurrentUser;
            model.Timestamp = DateTime.Now;
        }
    }
}
