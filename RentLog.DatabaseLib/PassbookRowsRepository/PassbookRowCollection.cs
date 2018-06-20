using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.PassbookRowsRepository
{
    internal class PassbookRowCollection : NamedCollectionBase<PassbookRowDTO>
    {
        internal PassbookRowCollection(int bankAcctID, SharedLiteDB sharedLiteDB) 
            : base(GetCollectionName(bankAcctID), sharedLiteDB)
        {
        }


        private static string GetCollectionName(int bankAcctID)
            => $"Account{bankAcctID}_SoaRows";


        protected override void EnsureIndeces(LiteCollection<PassbookRowDTO> coll)
        {
            coll.EnsureIndex(_ => _.DateOffset, false);
        }
    }
}
