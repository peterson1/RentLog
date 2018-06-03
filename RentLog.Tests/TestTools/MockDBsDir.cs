using Moq;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.Tests.TestTools
{
    class MockDBsDir : ITenantDBsDir
    {
        public Mock<ICollectionsDir> MoqCollectionsDir { get; } = new Mock<ICollectionsDir>();
        public Mock<IBalanceDB>      MoqBalanceDB      { get; } = new Mock<IBalanceDB>();

        public MarketStateDB   MarketState { get; } = new MockMarketState();

        public ICollectionsDir Collections => MoqCollectionsDir.Object;
        public IBalanceDB      Balances    => MoqBalanceDB.Object;
    }
}
