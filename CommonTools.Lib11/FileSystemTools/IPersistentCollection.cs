using System.Collections.Generic;

namespace CommonTools.Lib11.FileSystemTools
{
    public interface IPersistentCollection<T>
    {
        void            Clear     ();
        void            Set       (IEnumerable<T> list);
        bool            Any       ();
        IEnumerable<T>  Enumerate ();
    }
}
