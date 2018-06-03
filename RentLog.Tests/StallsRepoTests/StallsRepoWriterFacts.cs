using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.Tests.TestTools;
using System.Collections.Generic;
using Xunit;

namespace RentLog.Tests.StallsRepoTests
{
    [Trait("Stalls Repo", "Solitary")]
    public class StallsRepoWriterFacts
    {
        [Fact(DisplayName = "Insert Rejects duplicate name")]
        public void InsertRejectsduplicatestallname()
        {
            var moq   = new Mock<ISimpleRepo<StallDTO>>();
            var sut   = new StallsRepo1(moq.Object, null);
            var stall = StallDTO.Named("sample");

            moq.Setup(_ => _.GetAll())
                .Returns(new List<StallDTO> { stall });

            sut.Invoking(_ => _.Insert(stall))
                .Should().Throw<DuplicateRecordsException>();
        }


        [Fact(DisplayName = "Update Accepts Unique name")]
        public void UpdateAcceptsUniquename()
        {
            var moq   = new Mock<ISimpleRepo<StallDTO>>();
            var sut   = new StallsRepo1(moq.Object, null);
            var rec1  = new StallDTO { Id = 1, Name = "Sample 1" };
            var rec2a = new StallDTO { Id = 2, Name = "Sample 2" };
            var rec2b = rec2a.ShallowClone<StallDTO>();

            moq.Setup(_ => _.GetAll())
                .Returns(new List<StallDTO> { rec1, rec2a });

            rec2b.Name = rec2a.Name + " changed";
            sut.Update(rec2b);
        }


        [Fact(DisplayName = "Update Rejects duplicate name")]
        public void UpdateRejectsduplicatestallname()
        {
            var moq  = new Mock<ISimpleRepo<StallDTO>>();
            var sut  = new StallsRepo1(moq.Object, null);
            var rec1 = new StallDTO { Id = 1, Name = "Sample 1" };
            var rec2 = new StallDTO { Id = 2, Name = "Sample 2" };
            var recX = rec2.ShallowClone<StallDTO>();

            moq.Setup(_ => _.GetAll())
                .Returns(new List<StallDTO> { rec1, rec2 });

            recX.Name = rec1.Name;

            sut.Invoking(_ => _.Update(recX))
                .Should().Throw<DuplicateRecordsException>();
        }


        [Fact(DisplayName = "Can't Delete if Occupied")]
        public void CantDeleteifOccupied()
        {
            var moq   = new Mock<ISimpleRepo<StallDTO>>();
            var db    = new MockMarketState();
            var sut   = new StallsRepo1(moq.Object, db);
            var stall = StallDTO.Named("sample");

            db.MoqActiveLeases.Setup(_ => _.GetAll())
                .Returns(new List<LeaseDTO>
                    { new LeaseDTO { Stall = stall } });

            sut.Invoking(_ => _.Delete(stall)).Should()
                .Throw<InvalidDeletionException>();
        }


        [Fact(DisplayName = "Update Accepts non-name edit")]
        public void UpdateAcceptsnonnameedit()
        {
            var moq = new Mock<ISimpleRepo<StallDTO>>();
            var sut = new StallsRepo1(moq.Object, null);
            var rec = new StallDTO { Id = 1, Name = "Sample 1" };

            moq.Setup(_ => _.GetAll())
                .Returns(new List<StallDTO> { rec });

            rec.Name = rec.Name + " changed";
            sut.Update(rec);
        }
    }
}
