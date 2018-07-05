using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using Xunit;

namespace RentLog.Tests.IntendedColxnsRepoTests
{
    [Trait("IntentedColxns Repo", "Solitary")]
    public class IntendedColxnsRepoFacts
    {
        [Fact(DisplayName = "Rejects invalid Id")]
        public void ValidateszeroId()
        {
            var moq = new Mock<ISimpleRepo<IntendedColxnDTO>>();
            var sut = new IntendedColxnsRepo1(moq.Object);
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


        [Fact(DisplayName = "Rejects invalid PR #")]
        public void Rejectsblankdepositslip()
        {
            var moq = new Mock<ISimpleRepo<IntendedColxnDTO>>();
            var sut = new IntendedColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.PRNumber = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.PRNumber = 123;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.PRNumber = -456;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects null Lease")]
        public void RejectsnullBankAccount()
        {
            var moq = new Mock<ISimpleRepo<IntendedColxnDTO>>();
            var sut = new IntendedColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Lease = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Lease = new LeaseDTO { Id = 123 };
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects null Targets")]
        public void RejectsnullTargets()
        {
            var moq = new Mock<ISimpleRepo<IntendedColxnDTO>>();
            var sut = new IntendedColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Targets = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Targets = new BillAmounts();
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects null Actuals")]
        public void RejectsnullActuals()
        {
            var moq = new Mock<ISimpleRepo<IntendedColxnDTO>>();
            var sut = new IntendedColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Actuals = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Actuals = new BillAmounts { Rent = 456 };
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects invalid actuals")]
        public void Rejectzeroamount()
        {
            var moq = new Mock<ISimpleRepo<IntendedColxnDTO>>();
            var sut = new IntendedColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Actuals.Rent = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Actuals.Rent = 123;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Actuals.Rent = -456;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private IntendedColxnDTO ValidSampleDTO()
            => new IntendedColxnDTO
            {
                Id       = 0,
                PRNumber = 456,
                Lease    = new LeaseDTO { Id = 123 },
                Actuals  = new BillAmounts { Rent = 456 },
                Targets  = new BillAmounts { Rent = 456 }
            };
    }
}
