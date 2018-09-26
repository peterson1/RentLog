using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.JournalsRepository
{
    internal class JournalVouchersCollection : NamedCollection<JournalVoucherDTO>
    {
        internal const string COLXN_NAME = "JournalVoucherDTO";

        internal JournalVouchersCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }


        protected override void EnsureIndeces(LiteCollection<JournalVoucherDTO> coll)
        {
            coll.EnsureIndex(_ => _.DateOffset, false);
            coll.EnsureIndex(_ => _.SerialNum, true);
        }
    }
}
