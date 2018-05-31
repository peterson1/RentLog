using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.GLAccountsRepository
{
    internal class GLAccountsCollection : NamedCollectionBase<GLAccountDTO>, ISimpleRepo<GLAccountDTO>
    {
        internal const string COLXN_NAME = "GLAccountDTO";


        public GLAccountsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
