using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.Tests.TestTools;
using Xunit;

namespace RentLog.Tests.LeasesTests
{
    [Trait("Inactive Leases Repo", "Solitary")]
    public class InactiveLeasesRepoWriterFacts
    {
        [Fact(DisplayName = "Non-Active Insert Fails")]
        public void NonActiveInsertFails()
        {
            var arg = new MockMarketState();
            var moq = new Mock<ISimpleRepo<InactiveLeaseDTO>>();
            var sut = new InactiveLeasesRepo1(moq.Object, arg);
            var lse = new InactiveLeaseDTO();

            arg.MoqActiveLeases.Setup(_
                => _.HasId(lse.Id)).Returns(false);

            sut.Invoking(_ => _.Insert(lse))
                .Should().Throw<InvalidInsertionException>();
        }


        [Fact(DisplayName = "Insert returns rec with same ID")]
        public void ActiveInsertionreturnsrecwithsameID()
        {
            var arg = new MockMarketState();
            var moq = new Mock<ISimpleRepo<InactiveLeaseDTO>>();
            var sut = new InactiveLeasesRepo1(moq.Object, arg);
            var lse = new InactiveLeaseDTO { Id = 1234 };

            arg.MoqActiveLeases.SetupSequence(_
                => _.HasId(lse.Id)).Returns(true)
                                   .Returns(false);

            arg.MoqBalanceDB.Setup(_ => _.GetRepo(lse.Id))
                .Returns(Mock.Of<IDailyBillsRepo>());

            moq.Setup(_ => _.Insert(lse)).Returns(lse.Id);

            var rId = sut.Insert(lse);
            rId.Should().Be(lse.Id);
        }


        [Fact(DisplayName = "Insert removes from Actives after Save")]
        public void InsertremovesfromActivesafterSave()
        {
            var arg = new MockMarketState();
            var moq = new Mock<ISimpleRepo<InactiveLeaseDTO>>();
            var sut = new InactiveLeasesRepo1(moq.Object, arg);
            var lse = new InactiveLeaseDTO { Id = 1234 };

            arg.MoqActiveLeases.SetupSequence(_
                => _.HasId(lse.Id)).Returns(true)
                                   .Returns(false);

            arg.MoqBalanceDB.Setup(_ => _.GetRepo(lse.Id))
                .Returns(Mock.Of<IDailyBillsRepo>());

            sut.Insert(lse);

            arg.MoqActiveLeases.Verify(_ 
                => _.Delete(lse.Id), Times.Once());
        }


        [Fact(DisplayName = "Error if record undeleted from Actives")]
        public void ErrorifrecordundeletedforActives()
        {
            var arg = new MockMarketState();
            var moq = new Mock<ISimpleRepo<InactiveLeaseDTO>>();
            var sut = new InactiveLeasesRepo1(moq.Object, arg);
            var lse = new InactiveLeaseDTO { Id = 1234 };

            arg.MoqActiveLeases.SetupSequence(_
                => _.HasId(lse.Id)).Returns(true)
                                   .Returns(true);

            sut.Invoking(_ => _.Insert(lse))
                .Should().Throw<InvalidStateException>();
        }


        [Fact(DisplayName = "Insert calls BatchBalUpdate after Save")]
        public void InsertcallsBatchBalUpdateafterSave()
        {
            var arg = new MockMarketState();
            var moq = new Mock<ISimpleRepo<InactiveLeaseDTO>>();
            var bal = new Mock<IDailyBillsRepo>();
            var sut = new InactiveLeasesRepo1(moq.Object, arg);
            var lse = new InactiveLeaseDTO { Id = 1234 };

            arg.MoqActiveLeases.SetupSequence(_
                => _.HasId(lse.Id)).Returns(true)
                                   .Returns(false);
            arg.MoqBalanceDB.Setup(_ 
                => _.GetRepo(lse.Id)).Returns(bal.Object);

            sut.Insert(lse);

            bal.Verify(_ => _
                .UpdateFrom(lse.DeactivatedDate), Times.Once);
        }
    }
}
