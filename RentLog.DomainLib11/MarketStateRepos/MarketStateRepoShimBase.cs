using CommonTools.Lib11.DatabaseTools;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public abstract class MarketStateRepoShimBase<T> : SimpleRepoShimBase<T>
    {
        protected IMarketStateDB _db;


        public MarketStateRepoShimBase(ISimpleRepo<T> simpleRepo, IMarketStateDB marketStateDB) : base(simpleRepo)
        {
            _db = marketStateDB;
        }
    }
}
