using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.BankAccountsRepository
{
    internal class BankAccountsCollection : NamedCollection<BankAccountDTO>
    {
        internal const string COLXN_NAME = "BankAccountModel";


        internal BankAccountsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
