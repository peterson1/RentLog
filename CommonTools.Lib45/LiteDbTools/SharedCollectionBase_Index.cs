using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.FileSystemTools;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CommonTools.Lib45.LiteDbTools
{
    public abstract partial class SharedCollectionBase<T>
        where T : IDocumentDTO
    {
        private bool _hasCustomIndeces = true;


        //private void EnsureIndecesBeforeRead()
        //{
        //    if (!_hasCustomIndeces) return;

        //    using (var db = _db.OpenWrite())
        //        EnsureIndeces(GetCollection(db));
        //}


        protected virtual void EnsureIndeces(LiteCollection<T> coll)
        {
            _hasCustomIndeces = false;
        }


        private void EnsureIndecesAfterWrite(LiteCollection<T> coll)
        {
            if (_hasCustomIndeces)
                EnsureIndeces(coll);
        }
    }
}
