using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.DomainLib11.DataSources
{
    public interface ITenantDBsDir
    {
        MarketStateDB     MarketState   { get; }
        ICollectionsDir   Collections   { get; }
        IBalanceDB        Balances      { get; }
    }
}
