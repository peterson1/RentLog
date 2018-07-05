using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using Xunit;

namespace RentLog.Tests.OtherColxnsRepoTests
{
    [Trait("Other Colxns Repo", "Solitary")]
    public class OtherColxnsRepoFacts
    {
        [Fact(DisplayName = "Rejects invalid Id")]
        public void ValidateszeroId()
        {
            var moq = new Mock<ISimpleRepo<OtherColxnDTO>>();
            var sut = new OtherColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Id = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeFalse();

            obj.Id = 123;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Id = -456;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeFalse();
        }


        [Fact(DisplayName = "Rejects blank PR #")]
        public void Rejectsblankdepositslip()
        {
            var moq = new Mock<ISimpleRepo<OtherColxnDTO>>();
            var sut = new OtherColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.DocumentRef = "";
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.DocumentRef = "efg";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects invalid amount")]
        public void Rejectzeroamount()
        {
            var moq = new Mock<ISimpleRepo<OtherColxnDTO>>();
            var sut = new OtherColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Amount = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Amount = 123;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Amount = -456;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects null Collector")]
        public void RejectsnullBankAccount()
        {
            var moq = new Mock<ISimpleRepo<OtherColxnDTO>>();
            var sut = new OtherColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Collector = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Collector = new CollectorDTO();
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects null GLAccount")]
        public void RejectsinvalidBillCode()
        {
            var moq = new Mock<ISimpleRepo<OtherColxnDTO>>();
            var sut = new OtherColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.GLAccount = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.GLAccount = new GLAccountDTO();
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private OtherColxnDTO ValidSampleDTO() 
            => new OtherColxnDTO
            {
                Id          = 0,
                Amount      = 456,
                DocumentRef = "abc",
                Collector   = new CollectorDTO(),
                GLAccount   = new GLAccountDTO()
            };
    }
}
