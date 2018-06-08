using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DTOs;
using System;
using Xunit;

namespace RentLog.Tests.PreparedChequesRepoTests
{
    [Trait("Prepared Cheques Repo", "Solitary")]
    public class PreparedChequesRepoFacts
    {
        [Fact(DisplayName = "Rejects NULL request")]
        public void RejectsNULLrequest()
        {
            var moq = new Mock<ISimpleRepo<ChequeVoucherDTO>>();
            var sut = new PreparedChequesRepo1(moq.Object);
            var obj = ValidDraft();

            obj.Request = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Request = new FundRequestDTO { Amount = 123 };
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects blank request amount")]
        public void Rejectsblankamount()
        {
            var moq = new Mock<ISimpleRepo<ChequeVoucherDTO>>();
            var sut = new PreparedChequesRepo1(moq.Object);
            var obj = ValidDraft();

            obj.Request.Amount = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Request.Amount = 123456;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects invalid Cheque Number")]
        public void RejectsinvalidChequeNumber()
        {
            var moq = new Mock<ISimpleRepo<ChequeVoucherDTO>>();
            var sut = new PreparedChequesRepo1(moq.Object);
            var obj = ValidDraft();

            obj.ChequeNumber = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.ChequeNumber = -123;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.ChequeNumber = 123;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects invalid Cheque Date")]
        public void RejectsinvalidChequeDate()
        {
            var moq = new Mock<ISimpleRepo<ChequeVoucherDTO>>();
            var sut = new PreparedChequesRepo1(moq.Object);
            var obj = ValidDraft();

            obj.ChequeDate = DateTime.MinValue;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.ChequeDate = DateTime.Now;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private ChequeVoucherDTO ValidDraft()
            => new ChequeVoucherDTO
            {
                Id = 123,
                Request = new FundRequestDTO
                {
                    Amount = 1234
                },
                ChequeDate = DateTime.Now,
                ChequeNumber = 12345
            };
    }
}
