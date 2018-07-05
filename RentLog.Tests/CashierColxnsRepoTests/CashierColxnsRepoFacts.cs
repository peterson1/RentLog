using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using Xunit;

namespace RentLog.Tests.CashierColxnsRepoTests
{
    [Trait("Cashier Colxns Repo", "Solitary")]
    public class CashierColxnsRepoFacts
    {
        [Fact(DisplayName = "Rejects invalid Id")]
        public void ValidateszeroId()
        {
            var moq = new Mock<ISimpleRepo<CashierColxnDTO>>();
            var sut = new CashierColxnsRepo1(moq.Object);
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
            var moq = new Mock<ISimpleRepo<CashierColxnDTO>>();
            var sut = new CashierColxnsRepo1(moq.Object);
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
            var moq = new Mock<ISimpleRepo<CashierColxnDTO>>();
            var sut = new CashierColxnsRepo1(moq.Object);
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


        [Fact(DisplayName = "Rejects null Lease")]
        public void RejectsnullBankAccount()
        {
            var moq = new Mock<ISimpleRepo<CashierColxnDTO>>();
            var sut = new CashierColxnsRepo1(moq.Object);
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


        [Fact(DisplayName = "Rejects invalid BillCode")]
        public void RejectsinvalidBillCode()
        {
            var moq = new Mock<ISimpleRepo<CashierColxnDTO>>();
            var sut = new CashierColxnsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.BillCode = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.BillCode = BillCode.Other;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private CashierColxnDTO ValidSampleDTO() 
            => new CashierColxnDTO
            {
                Id          = 0,
                Amount      = 456,
                Lease       = new LeaseDTO { Id = 123 },
                DocumentRef = "abc",
                BillCode    = BillCode.Rent
            };
    }
}
