using CommonTools.Lib11.DatabaseTools;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public abstract class MarketStateRepoShimBase<T> : SimpleRepoShimBase<T>
    {
        protected MarketStateDB _db;


        public MarketStateRepoShimBase(ISimpleRepo<T> simpleRepo, MarketStateDB marketStateDB) : base(simpleRepo)
        {
            _db = marketStateDB;
        }
    }
}
