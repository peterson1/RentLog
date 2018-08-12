using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.FundRequestsRepository
{
    internal class InactiveRequestsCollection : NamedCollectionBase<FundRequestDTO>, ISimpleRepo<FundRequestDTO>
    {
        internal const string COLXN_NAME = "FundReqs_Inactive";


        internal InactiveRequestsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }


        protected override void EnsureIndeces(LiteCollection<FundRequestDTO> coll)
        {
            coll.EnsureIndex(_ => _.SerialNum);
            coll.EnsureIndex(_ => _.RequestDate);
            coll.EnsureIndex(_ => _.BankAccountId);
        }
    }
}
