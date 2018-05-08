using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Repositories;
using RentLog.Tests.TestTools;
using System.Collections.Generic;
using Xunit;

namespace RentLog.Tests.StallsRepoTests
{
    [Trait("Stalls Repo", "Solitary")]
    public class StallsRepoFacts
    {
        [Fact(DisplayName = "Rejects duplicate stall name")]
        public void Rejectsduplicatestallname()
        {
            var moq = new Mock<ISimpleRepo<StallDTO>>();
            var sut = new StallsRepo1(moq.Object, null);
            var stall = StallDTO.Named("sample");

            moq.Setup(_ => _.GetAll())
                .Returns(new List<StallDTO> { stall });

            sut.Invoking(_ => _.Insert(stall))
                .Should().Throw<DuplicateRecordsException>();
        }


        [Fact(DisplayName = "Can't Delete if Occupied")]
        public void CantDeleteifOccupied()
        {
            var moq   = new Mock<ISimpleRepo<StallDTO>>();
            var db    = new MockDB();
            var sut   = new StallsRepo1(moq.Object, db);
            var stall = StallDTO.Named("sample");

            db.MoqActiveLeases.Setup(_ => _.GetAll())
                .Returns(new List<LeaseDTO>
                    { new LeaseDTO { Stall = stall } });

            sut.Invoking(_ => _.Delete(stall)).Should()
                .Throw<InvalidDeletionException>();
        }
    }
}
