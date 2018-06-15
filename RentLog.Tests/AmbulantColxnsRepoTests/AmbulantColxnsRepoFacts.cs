using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using Xunit;

namespace RentLog.Tests.AmbulantColxnsRepoTests
{
    [Trait("Ambulant Colxns Repo", "Solitary")]
    public class AmbulantColxnsRepoFacts
    {
        [Fact(DisplayName = "Rejects invalid Id")]
        public void ValidateszeroId()
        {
            var moq = new Mock<ISimpleRepo<AmbulantColxnDTO>>();
            var sut = new AmbulantColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Id = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeFalse();

            obj.Id = 123;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Id = -456;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeFalse();
        }


        [Fact(DisplayName = "Rejects blank PR #")]
        public void Rejectsblankdepositslip()
        {
            var moq = new Mock<ISimpleRepo<AmbulantColxnDTO>>();
            var sut = new AmbulantColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.PRNumber = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.PRNumber = 456;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects invalid amount")]
        public void Rejectzeroamount()
        {
            var moq = new Mock<ISimpleRepo<AmbulantColxnDTO>>();
            var sut = new AmbulantColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Amount = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Amount = 123;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Amount = -456;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects blank Received-From")]
        public void RejectsnullBankAccount()
        {
            var moq = new Mock<ISimpleRepo<AmbulantColxnDTO>>();
            var sut = new AmbulantColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.ReceivedFrom = "";
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.ReceivedFrom = "Someone";
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private AmbulantColxnDTO ValidSampleDTO() 
            => new AmbulantColxnDTO
            {
                Id           = 123,
                Amount       = 456,
                PRNumber     = 789,
                ReceivedFrom = "a tenant"
            };
    }
}
