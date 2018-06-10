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
            var obj = ValidCheque();

            obj.Request = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Request = ValidRequest();
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects blank request amount")]
        public void Rejectsblankamount()
        {
            var moq = new Mock<ISimpleRepo<ChequeVoucherDTO>>();
            var sut = new PreparedChequesRepo1(moq.Object);
            var obj = ValidCheque();

            obj.Request.Amount = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Request.Amount = 123456;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects invalid bank acct Id")]
        public void RejectsinvalidbankacctId()
        {
            var moq = new Mock<ISimpleRepo<ChequeVoucherDTO>>();
            var sut = new PreparedChequesRepo1(moq.Object);
            var obj = ValidCheque();

            obj.Request.BankAccountId = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Request.BankAccountId = -123;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Request.BankAccountId = 1234;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects invalid Cheque Number")]
        public void RejectsinvalidChequeNumber()
        {
            var moq = new Mock<ISimpleRepo<ChequeVoucherDTO>>();
            var sut = new PreparedChequesRepo1(moq.Object);
            var obj = ValidCheque();

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
            var obj = ValidCheque();

            obj.ChequeDate = DateTime.MinValue;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.ChequeDate = DateTime.Now;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private ChequeVoucherDTO ValidCheque()
            => new ChequeVoucherDTO
            {
                Id           = 123,
                Request      = ValidRequest(),
                ChequeDate   = DateTime.Now,
                ChequeNumber = 12345
            };

        private static FundRequestDTO ValidRequest() 
            => new FundRequestDTO
        {
            Amount        = 1234,
            BankAccountId = 1,
        };
    }
}
