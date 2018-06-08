using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.FundRequestsRepository
{
    internal class ActiveRequestsCollection : NamedCollectionBase<FundRequestDTO>, ISimpleRepo<FundRequestDTO>
    {
        internal const string COLXN_NAME = "FundReqs_Active";


        internal ActiveRequestsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
