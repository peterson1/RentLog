using System;
using System.Collections.Generic;

namespace CommonTools.Lib11.DatabaseTools
{
    public interface ISimpleRepo<T>
    {
        event EventHandler<T> ContentChanged;

        List<T> GetAll  ();
        T       Find    (int recordId, bool errorIfMissing);
        bool    HasName (string recordName, string field = "Name");
        Dictionary<int, T> ToDictionary();

        int     Insert  (T newRecord);
        bool    Update  (T changedRecord);
        bool    Upsert  (T record);
        bool    Delete  (T record);
    }
}
