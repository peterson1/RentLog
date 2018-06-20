using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommonTools.Lib11.DatabaseTools
{
    public interface ISimpleRepo<T>
    {
        event EventHandler<T> ContentChanged;

        bool      Any      ();
        List<T>   GetAll   ();
        T         Find     (int recordId, bool errorIfMissing);
        List<T>   Find     (Expression<Func<T, bool>> predicate);
        T         Earliest ();
        T         Latest   ();
        int       Max      (Expression<Func<T, int>> getter);
        DateTime  Max      (Expression<Func<T, DateTime>> getter);
        bool      HasId    (int recordId);
        bool      HasName  (string recordName, string field = "Name");

        Dictionary<int, T> ToDictionary();

        int     Insert   (T newRecord);
        void    Insert   (IEnumerable<T> records, bool doValidate);
                         
        bool    Update   (T changedRecord);
        void    Update   (IEnumerable<T> records, bool doValidate);
                         
        bool    Upsert   (T record);
        bool    Delete   (T record);
        bool    Delete   (int recordId);

        void  Drop              ();
        void  DropAndInsert     (IEnumerable<T> records, bool doValidate);

        bool  IsValidForInsert  (T draft, out string whyInvalid);
        bool  IsValidForUpdate  (T record, out string whyInvalid);
        bool  IsValidForDelete  (T record, out string whyInvalid);
    }
}
