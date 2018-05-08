using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class BalancesDBFolder : BalanceDBsBase
    {
        public BalancesDBFolder(string marketDbFilePath, string currentUser)
        {
        }


        protected override DateTime GetLastClosedDate()
        {
            throw new NotImplementedException();
        }


        protected override IDailyBillsRepo GetRepo(LeaseDTO lse)
        {
            throw new NotImplementedException();
        }
    }
}
