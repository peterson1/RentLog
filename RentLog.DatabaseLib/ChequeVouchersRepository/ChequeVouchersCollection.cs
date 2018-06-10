using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.ChequeVouchersRepository
{
    internal class ChequeVouchersCollection : NamedCollectionBase<ChequeVoucherDTO>, ISimpleRepo<ChequeVoucherDTO>
    {
        internal const string COLXN_NAME = "RequestedCheques_Active";


        internal ChequeVouchersCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }


        // LiteDB currently doesn't support indexing Nullable<DateTime>
        //protected override void EnsureIndeces(LiteCollection<ChequeVoucherDTO> coll)
        //    => coll.EnsureIndex(_ => _.IssuedDate, false);
    }
}
