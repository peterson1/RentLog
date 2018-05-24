using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.Tests.TestTools;
using Xunit;

namespace RentLog.Tests.LeasesRepoTests
{
    [Trait("Active Leases Repo", "Solitary")]
    public class ActiveLeasesRepoWriterFacts
    {
        [Fact(DisplayName = "Undeactivated Delete fails")]
        public void CallingDeletefails()
        {
            var arg = new MockDB();
            var moq = new Mock<ISimpleRepo<LeaseDTO>>();
            var sut = new ActiveLeasesRepo1(moq.Object, arg);
            var lse = new LeaseDTO();

            arg.MoqInactiveLeases.Setup(_
                => _.HasId(lse.Id)).Returns(false);

            sut.Invoking(_ => _.Delete(lse))
                .Should().Throw<InvalidDeletionException>();
        }


        [Fact(DisplayName = "Deactivated Delete returns true")]
        public void TestMethod()
        {
            var arg = new MockDB();
            var moq = new Mock<ISimpleRepo<LeaseDTO>>();
            var lse = new LeaseDTO();
            var sut = new ActiveLeasesRepo1(moq.Object, arg);

            arg.MoqInactiveLeases.Setup(_ 
                => _.HasId(lse.Id)).Returns(true);

            moq.Setup(_ => _.Delete(lse)).Returns(true);

            sut.Delete(lse).Should().BeTrue();
        }
    }
}
