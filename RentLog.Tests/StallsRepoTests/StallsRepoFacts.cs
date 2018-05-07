using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.LiteDbTools;
using FluentAssertions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.Repositories;
using System.IO;
using Xunit;

namespace RentLog.Tests.StallsRepoTests
{
    [Trait("Stalls Repo", "Solitary")]
    public class StallsRepoFacts
    {
        [Fact(DisplayName = "Rejects duplicate stall name")]
        public void Rejectsduplicatestallname()
        {
            var db  = new SharedLiteDB(new MemoryStream(), "");
            var sut = new StallsRepo(db);
            sut.Insert(new StallDTO { Name = "a" });

            sut.Invoking(_ => _.Insert(new StallDTO
            {
                Name = "a"
            }
            )).Should().Throw<DuplicateRecordsException>();
        }


        [Fact(DisplayName = "Can't Delete if Occupied")]
        public void CantDeleteifOccupied()
        {
            var db  = new SharedLiteDB(new MemoryStream(), "");
            var sut = new StallsRepo(db);
            var stall = new StallDTO { Name = "a" };
            sut.Insert(stall);
            SetOccupant(stall);

            sut.Invoking(_ => _.Delete(stall)).Should()
                .Throw<InvalidDeletionException>();
        }
    }
}
