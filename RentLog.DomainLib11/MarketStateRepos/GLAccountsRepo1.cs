using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class GLAccountsRepo1 : MarketStateRepoShimBase<GLAccountDTO>, IGLAccountsRepo
    {
        public GLAccountsRepo1(ISimpleRepo<GLAccountDTO> simpleRepo, MarketStateDbBase marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }


        public List<GLAccountDTO> AllWithCashInBanks()
        {
            var list = GetAll();
            var cID  = -1;

            foreach (var bank in _db.BankAccounts.GetAll())
                list.Add(GLAccountDTO.CashInBank(bank, cID--));

            return ToSortedList(list);
        }


        protected override IEnumerable<GLAccountDTO> ToSorted(IEnumerable<GLAccountDTO> items)
            => items.OrderBy(_ => _.Name);
    }
}
