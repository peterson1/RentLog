using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.DailyColxnsRepository
{
    internal class OtherColxnsCollection : NamedCollectionBase<OtherColxnDTO>
    {
        private const string COLXN_NAME = "OtherColxnDTO";

        internal OtherColxnsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
