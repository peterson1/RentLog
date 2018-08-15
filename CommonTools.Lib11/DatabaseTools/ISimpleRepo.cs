using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommonTools.Lib11.DatabaseTools
{
    public interface ISimpleRepo<T> : IReadOnlyRepo<T>, ISimplerRepo<T>
    {
        event EventHandler<T> ContentChanged;

        void    Insert   (IEnumerable<T> records, bool doValidate);
        void    Update   (IEnumerable<T> records, bool doValidate);
        void    Upsert   (IEnumerable<T> records, bool doValidate);
                         
        bool    Upsert   (T record);
        bool    Delete   (int recordId);
        bool    Delete   (List<T> records);

        void  Drop              ();
        void  DropAndInsert     (IEnumerable<T> records, bool doValidate);
    }
}
