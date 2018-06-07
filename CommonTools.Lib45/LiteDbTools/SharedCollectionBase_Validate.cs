using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;

namespace CommonTools.Lib45.LiteDbTools
{
    public abstract partial class SharedCollectionBase<T> : ISimpleRepo<T>
        where T : IDocumentDTO
    {
        public virtual bool IsValidForInsert(T draft, out string whyInvalid)
        {
            whyInvalid = string.Empty;
            return true;
        }

        public virtual bool IsValidForUpdate(T record, out string whyInvalid)
        {
            whyInvalid = string.Empty;
            return true;
        }

        public virtual bool IsValidForDelete(T record, out string whyInvalid)
        {
            whyInvalid = string.Empty;
            return true;
        }
    }
}
