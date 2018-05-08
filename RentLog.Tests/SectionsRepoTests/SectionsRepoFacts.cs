using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.Tests.TestTools;
using System.Collections.Generic;
using Xunit;

namespace RentLog.Tests.SectionsRepoTests
{
    [Trait("Sections Repo", "Solitary")]
    public class SectionsRepoFacts
    {
        [Fact(DisplayName = "Insert Rejects duplicate name")]
        public void InsertRejectsduplicateSectionname()
        {
            var moq = new Mock<ISimpleRepo<SectionDTO>>();
            var sut = new SectionsRepo1(moq.Object, null);
            var sec = SectionDTO.Named("sample");

            moq.Setup(_ => _.GetAll())
                .Returns(new List<SectionDTO> { sec });

            sut.Invoking(_ => _.Insert(sec))
                .Should().Throw<DuplicateRecordsException>();
        }


        [Fact(DisplayName = "Update Accepts Unique name")]
        public void UpdateAcceptsUniquename()
        {
            var moq   = new Mock<ISimpleRepo<SectionDTO>>();
            var sut   = new SectionsRepo1(moq.Object, null);
            var rec1  = new SectionDTO { Id = 1, Name = "Sample 1" };
            var rec2a = new SectionDTO { Id = 2, Name = "Sample 2" };
            var rec2b = rec2a.ShallowClone<SectionDTO>();

            moq.Setup(_ => _.GetAll())
                .Returns(new List<SectionDTO> { rec1, rec2a });

            rec2b.Name = rec2a.Name + " changed";
            sut.Update(rec2b);
        }


        [Fact(DisplayName = "Update Rejects duplicate name")]
        public void UpdateRejectsduplicatestallname()
        {
            var moq  = new Mock<ISimpleRepo<SectionDTO>>();
            var sut  = new SectionsRepo1(moq.Object, null);
            var rec1 = new SectionDTO { Id = 1, Name = "Sample 1" };
            var rec2 = new SectionDTO { Id = 2, Name = "Sample 2" };
            var recX = rec2.ShallowClone<SectionDTO>();

            moq.Setup(_ => _.GetAll())
                .Returns(new List<SectionDTO> { rec1, rec2 });

            recX.Name = rec1.Name;

            sut.Invoking(_ => _.Update(recX))
                .Should().Throw<DuplicateRecordsException>();
        }
    }
}
