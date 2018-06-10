using FluentAssertions;
using Moq;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DTOs;
using System;
using Xunit;

namespace RentLog.Tests.ChequeVouchersDBTests
{
    [Trait("Cheque Vouchers DB", "Solitary")]
    public class ChequeVouchersDBFacts
    {
        [Fact(DisplayName = "Set As Prepared")]
        public void SetAsPrepared()
        {
            var activs  = new Mock<IFundRequestsRepo>();
            var inactvs = new Mock<IFundRequestsRepo>();
            var cheques = new Mock<IChequeVouchersRepo>();
            var req     = CreateValidRequest();
            var sut     = new ChequeVouchersDB
            {
                ActiveRequests   = activs.Object,
                InactiveRequests = inactvs.Object,
                PreparedCheques  = cheques.Object
            };

            sut.SetAs_Prepared(req, DateTime.Now, 123);

            cheques.Verify(_ => _.Insert(It.IsAny<ChequeVoucherDTO>()), Times.Once);
            inactvs.Verify(_ => _.Insert(req), Times.Once);
            activs .Verify(_ => _.Delete(req), Times.Once);
        }


        [Fact(DisplayName = "Set As Issued")]
        public void SetAsIssued()
        {
            var repo = new Mock<IChequeVouchersRepo>();
            var chq  = CreateValidVoucher();
            var date = DateTime.Now;
            var dude = "Mr. Recipient";
            var sut  = new ChequeVouchersDB{ PreparedCheques = repo.Object };

            sut.SetAs_Issued(chq, date, dude);

            chq.IssuedDate.Should().Be(date);
            chq.IssuedTo  .Should().Be(dude);
            repo.Verify(_ => _.Update(chq), Times.Once);
        }


        [Fact(DisplayName = "Set As Taken Back")]
        public void SetAsTakenBack()
        {
            var repo = new Mock<IChequeVouchersRepo>();
            var chq  = CreateValidVoucher();
            var sut  = new ChequeVouchersDB{ PreparedCheques = repo.Object };

            sut.SetAs_TakenBack(chq);

            chq.IssuedDate.Should().BeNull();
            chq.IssuedTo  .Should().BeNullOrWhiteSpace();
            repo.Verify(_ => _.Update(chq), Times.Once);
        }


        private FundRequestDTO CreateValidRequest()
            => new FundRequestDTO
            {
                Id     = 1234,
                Amount = 4567
            };


        private ChequeVoucherDTO CreateValidVoucher()
            => new ChequeVoucherDTO
            {
                Request = CreateValidRequest(),
            };
    }
}
