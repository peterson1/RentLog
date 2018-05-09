using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.CollectorsRepository
{
    internal class CollectorsCollection : NamedCollectionBase<CollectorDTO>
    {
        internal const string COLXN_NAME = "CollectorModel";


        internal CollectorsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
