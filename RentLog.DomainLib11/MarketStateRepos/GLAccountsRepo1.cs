using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class GLAccountsRepo1 : MarketStateRepoShimBase<GLAccountDTO>, IGLAccountsRepo
    {
        public GLAccountsRepo1(ISimpleRepo<GLAccountDTO> simpleRepo, MarketStateDB marketStateDB) : base(simpleRepo, marketStateDB)
        {
        }
    }
}
