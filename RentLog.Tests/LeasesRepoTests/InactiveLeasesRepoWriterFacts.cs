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
    [Trait("Inactive Leases Repo", "Solitary")]
    public class InactiveLeasesRepoWriterFacts
    {
        [Fact(DisplayName = "Non-Active Insert Fails")]
        public void NonActiveInsertFails()
        {
            var arg = new MockDB();
            var moq = new Mock<ISimpleRepo<InactiveLeaseDTO>>();
            var sut = new InactiveLeasesRepo1(moq.Object, arg);
            var lse = new InactiveLeaseDTO();

            arg.MoqActiveLeases.Setup(_
                => _.HasId(lse.Id)).Returns(false);

            sut.Invoking(_ => _.Insert(lse))
                .Should().Throw<InvalidInsertionException>();
        }


        [Fact(DisplayName = "Active Insertion returns rec with same ID")]
        public void ActiveInsertionreturnsrecwithsameID()
        {
            var arg = new MockDB();
            var moq = new Mock<ISimpleRepo<InactiveLeaseDTO>>();
            var sut = new InactiveLeasesRepo1(moq.Object, arg);
            var lse = new InactiveLeaseDTO { Id = 1234 };

            arg.MoqActiveLeases.Setup(_
                => _.HasId(lse.Id)).Returns(true);

            moq.Setup(_ => _.Insert(lse)).Returns(lse.Id);

            var rId = sut.Insert(lse);
            rId.Should().Be(lse.Id);
        }
    }
}
