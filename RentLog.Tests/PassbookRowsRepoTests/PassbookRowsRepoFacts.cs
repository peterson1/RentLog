using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using System;
using Xunit;

namespace RentLog.Tests.PassbookRowsRepoTests
{
    [Trait("PassbookRows Repo", "Solitary")]
    public class PassbookRowsRepoFacts
    {
        [Fact(DisplayName = "Rejects invalid Bank Acct ID")]
        public void RejectinvalidBankAcctID()
        {
            var acctId = 1234;
            var sut    = new TestClass(acctId);
            var chq    = CreateValidCheque(acctId);

            chq.Request.BankAccountId = 456;
            sut.Invoking(_ =>
            {
                _.InsertClearedCheque(chq, DateTime.Now);
            }
            ).Should().Throw<InvalidInsertionException>();

            chq.Request.BankAccountId = acctId;
            sut.InsertClearedCheque(chq, DateTime.Now);

            sut.MoqRepo.Verify(_ 
                => _.Insert(It.IsAny<PassbookRowDTO>()), Times.Once);
        }


        [Fact(DisplayName = "Rejects null request amount")]
        public void Rejectsnullrequestamount()
        {
            var acctId = 1234;
            var sut    = new TestClass(acctId);
            var chq    = CreateValidCheque(acctId);

            chq.Request.Amount = null;
            sut.Invoking(_ =>
            {
                _.InsertClearedCheque(chq, DateTime.Now);
            }
            ).Should().Throw<InvalidInsertionException>();

            chq.Request.Amount = 456;
            sut.InsertClearedCheque(chq, DateTime.Now);

            sut.MoqRepo.Verify(_
                => _.Insert(It.IsAny<PassbookRowDTO>()), Times.Once);
        }


        private FundRequestDTO CreateValidRequest(int bankAcctId) => new FundRequestDTO
        {
            Id            = 1234,
            Amount        = 4567,
            BankAccountId = bankAcctId,
        };


        private ChequeVoucherDTO CreateValidCheque(int bankAcctId) => new ChequeVoucherDTO
        {
            Request = CreateValidRequest(bankAcctId),
        };



        private class TestClass : VagrantRepoFacadeBase
        {
            public TestClass(int bankAccountId) : base(bankAccountId)
            {
            }


            public Mock<ISimpleRepo<PassbookRowDTO>> MoqRepo { get; } = new Mock<ISimpleRepo<PassbookRowDTO>>();


            protected override ISimpleRepo<PassbookRowDTO> ConnectToDB(string databasePath)
                => new PassbookRowsSimpleRepo(MoqRepo.Object);


            protected override string GetDatabasePath(DateTime date) => date.ToShortDateString();
        }
    }
}
