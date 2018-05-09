using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.DailyBillsRepository
{
    public class DailyBillsCollection : NamedCollectionBase<DailyBillDTO>
    {
        private const string COLXN_NAME = "DailyBillDTO";

        public DailyBillsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
