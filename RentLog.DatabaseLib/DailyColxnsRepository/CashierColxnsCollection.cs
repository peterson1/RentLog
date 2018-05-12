using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.DailyColxnsRepository
{
    internal class CashierColxnsCollection : NamedCollectionBase<CashierColxnDTO>
    {
        private const string COLXN_NAME = "CashierColxnDTO";

        internal CashierColxnsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
