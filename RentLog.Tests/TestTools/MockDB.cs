using Moq;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.Tests.TestTools
{
    class MockDB : MarketStateDB
    {
        public MockDB()
        {
            MoqStalls       = new Mock<IStallsRepo>();
            MoqSections     = new Mock<ISectionsRepo>();
            MoqActiveLeases = new Mock<IActiveLeasesRepo>();
        }

        public Mock<IStallsRepo>   MoqStalls       { get; }
        public Mock<ISectionsRepo> MoqSections     { get; }
        public Mock<IActiveLeasesRepo>   MoqActiveLeases { get; }


        public override IStallsRepo   Stalls       => MoqStalls.Object;
        public override ISectionsRepo Sections     => MoqSections.Object;
        public override IActiveLeasesRepo   ActiveLeases => MoqActiveLeases.Object;
    }
}
