using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.DatabaseLib.DailyColxnsRepository
{
    internal class CashierColxnsCollection : NamedCollectionBase<CashierColxnDTO>
    {
        private const string COLXN_NAME = "CashierColxnDTO";

        internal CashierColxnsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
