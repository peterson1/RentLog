using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class GLAccountsRepo1 : MarketStateRepoShimBase<GLAccountDTO>, IGLAccountsRepo
    {
        public GLAccountsRepo1(ISimpleRepo<GLAccountDTO> simpleRepo, MarketStateDB marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }


        protected override IEnumerable<GLAccountDTO> ToSorted(IEnumerable<GLAccountDTO> items)
            => items.OrderBy(_ => _.Name);
    }
}
