using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Repositories;
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

            moq.Setup(_ => _.GetAll())
                .Returns(new List<StallDTO> { NewStall("a") });

            sut.Invoking(_ => _.Insert(NewStall("a")))
                .Should().Throw<DuplicateRecordsException>();
        }


        private StallDTO NewStall(string name) => new StallDTO
        {
            Name = name
        };


        [Fact(DisplayName = "Can't Delete if Occupied")]
        public void CantDeleteifOccupied()
        {
            var moq = new Mock<ISimpleRepo<StallDTO>>();
            var sut = new StallsRepo1(moq.Object, null);
            var stall = new StallDTO { Name = "a" };
            sut.Insert(stall);
            //SetOccupant(stall);

            sut.Invoking(_ => _.Delete(stall)).Should()
                .Throw<InvalidDeletionException>();
        }
    }
}
