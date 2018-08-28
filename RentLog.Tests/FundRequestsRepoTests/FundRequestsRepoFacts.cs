using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace RentLog.Tests.FundRequestsRepoTests
{
    [Trait("Fund Requests Repo", "Solitary")]
    public class FundRequestsRepoFacts
    {
        [Fact(DisplayName = "Rejects Invalid serial num")]
        public void TestMethod00001()
        {
            var moq = new Mock<ISimpleRepo<FundRequestDTO>>();
            var sut = new FundRequestsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.SerialNum = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.SerialNum = 123;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.SerialNum = -456;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Invalid Bank Acct ID")]
        public void TestMethod00002()
        {
            var moq = new Mock<ISimpleRepo<FundRequestDTO>>();
            var sut = new FundRequestsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.BankAccountId = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.BankAccountId = 123;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.BankAccountId = -456;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Blank Payee")]
        public void TestMethod00003()
        {
            var moq = new Mock<ISimpleRepo<FundRequestDTO>>();
            var sut = new FundRequestsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Payee = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Payee = "Someone";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Blank Purpose")]
        public void TestMethod00004()
        {
            var moq = new Mock<ISimpleRepo<FundRequestDTO>>();
            var sut = new FundRequestsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Purpose = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Purpose = "reason";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Invalid Request Date")]
        public void TestMethod00005()
        {
            var moq = new Mock<ISimpleRepo<FundRequestDTO>>();
            var sut = new FundRequestsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.DateOffset = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.DateOffset = DateTime.Now.DaysSinceMin();
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Invalid Amount")]
        public void TestMethod00006()
        {
            var moq = new Mock<ISimpleRepo<FundRequestDTO>>();
            var sut = new FundRequestsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Amount = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Amount = 0;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
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


        [Fact(DisplayName = "Rejects imbalanced allocations")]
        public void TestMethod00007()
        {
            var moq = new Mock<ISimpleRepo<FundRequestDTO>>();
            var sut = new FundRequestsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Allocations[1].SubAmount = -788;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Allocations[1].SubAmount = -789;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects missing Cash-in-bank entry")]
        public void TestMethod00008()
        {
            var moq = new Mock<ISimpleRepo<FundRequestDTO>>();
            var sut = new FundRequestsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Allocations[0].Account.Id = 123;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Allocations[0].Account.Id = 0;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private FundRequestDTO ValidSampleDTO() => new FundRequestDTO
        {
            Id            = 0,
            SerialNum     = 123,
            BankAccountId = 456,
            Payee         = "Mr. Payee Dude",
            Purpose       = "To be Purposeful",
            DateOffset    = DateTime.Now.DaysSinceMin(),
            Amount        = 789,
            Allocations   = new List<AccountAllocation>
            {
                AccountAllocation.DefaultCashInBank
                    (new BankAccountDTO(), 789),

                new AccountAllocation
                {
                    Account   = new GLAccountDTO{ Id = 456, Name = "a GL Account"},
                    SubAmount = -789
                },
            }
        };
    }
}
