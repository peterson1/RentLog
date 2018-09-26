using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.DailyColxnsRepository
{
    internal class BankDepositsCollection : NamedCollection<BankDepositDTO>
    {
        private const string COLXN_NAME = "BankDepositDTO";

        internal BankDepositsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
