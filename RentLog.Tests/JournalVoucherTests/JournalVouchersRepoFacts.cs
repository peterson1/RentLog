using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.JournalVoucherRepos;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace RentLog.Tests.JournalVoucherTests
{
    [Trait("Journal Vouchers Repo", "Solitary")]
    public class JournalVouchersRepoFacts
    {
        [Fact(DisplayName = "Rejects invalid Id")]
        public void TestMethod00000()
        {
            var moq = new Mock<ISimpleRepo<JournalVoucherDTO>>();
            var sut = new JournalSoloShard1(moq.Object);
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


        [Fact(DisplayName = "Rejects Invalid serial num")]
        public void TestMethod00001()
        {
            var moq = new Mock<ISimpleRepo<JournalVoucherDTO>>();
            var sut = new JournalSoloShard1(moq.Object);
            var obj = ValidSampleDTO();

            obj.SerialNum = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.SerialNum = 123;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.SerialNum = -456;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Blank Description")]
        public void TestMethod00003()
        {
            var moq = new Mock<ISimpleRepo<JournalVoucherDTO>>();
            var sut = new JournalSoloShard1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Description = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Description = "some desc";
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Invalid Transaction Date")]
        public void TestMethod00005()
        {
            var moq = new Mock<ISimpleRepo<JournalVoucherDTO>>();
            var sut = new JournalSoloShard1(moq.Object);
            var obj = ValidSampleDTO();

            obj.DateOffset = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.DateOffset = DateTime.Now.DaysSinceMin();
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Invalid Amount")]
        public void TestMethod00006()
        {
            var moq = new Mock<ISimpleRepo<JournalVoucherDTO>>();
            var sut = new JournalSoloShard1(moq.Object);
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


        [Fact(DisplayName = "Rejects imbalanced allocations")]
        public void TestMethod00007()
        {
            var moq = new Mock<ISimpleRepo<JournalVoucherDTO>>();
            var sut = new JournalSoloShard1(moq.Object);
            var obj = ValidSampleDTO();

            obj.Allocations[1].SubAmount = -788;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Allocations[1].SubAmount = -789;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private JournalVoucherDTO ValidSampleDTO() => new JournalVoucherDTO
        {
            Id          = 101,
            SerialNum   = 123,
            Description = "sample JV",
            DateOffset  = DateTime.Now.DaysSinceMin(),
            Amount      = 789,
            Allocations = new List<AccountAllocation>
            {
                AccountAllocation.NewCredit(GLAccountDTO.Named("test GL 1"), 789),
                AccountAllocation.NewDebit (GLAccountDTO.Named("test GL 2"), 789),
            }
        };
    }
}
