using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.DailyColxnsRepository
{
    public class BalanceAdjsCollection : NamedCollectionBase<BalanceAdjustmentDTO>
    {
        private const string COLXN_NAME = "BalanceAdjustmentDTO";

        public BalanceAdjsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
