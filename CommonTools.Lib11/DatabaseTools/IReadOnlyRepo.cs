using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommonTools.Lib11.DatabaseTools
{
    public interface IReadOnlyRepo<T>
    {

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
        bool      TableExists();

        Dictionary<int, T> ToDictionary();
    }
}
