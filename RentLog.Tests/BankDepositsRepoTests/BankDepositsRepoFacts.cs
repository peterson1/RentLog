using CommonTools.Lib11.DatabaseTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using Xunit;

namespace RentLog.Tests.BankDepositsRepoTests
{
    [Trait("Bank Deps Repo", "Solitary")]
    public class BankDepositsRepoFacts
    {
        [Fact(DisplayName = "Rejects invalid Id")]
        public void ValidateszeroId()
        {
            var moq = new Mock<ISimpleRepo<BankDepositDTO>>();
            var sut = new BankDepositsRepo1(moq.Object);
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


        [Fact(DisplayName = "Rejects invalid amount")]
        public void Rejectzeroamount()
        {
            var moq = new Mock<ISimpleRepo<BankDepositDTO>>();
            var sut = new BankDepositsRepo1(moq.Object);
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


        [Fact(DisplayName = "Rejects null Bank Account")]
        public void RejectsnullBankAccount()
        {
            var moq = new Mock<ISimpleRepo<BankDepositDTO>>();
            var sut = new BankDepositsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.BankAccount = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.BankAccount = new BankAccountDTO();
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects blank deposit slip #")]
        public void Rejectsblankdepositslip()
        {
            var moq = new Mock<ISimpleRepo<BankDepositDTO>>();
            var sut = new BankDepositsRepo1(moq.Object);
            var obj = ValidSampleDTO();

            obj.DocumentRef = "";
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.DocumentRef = "efg";
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private BankDepositDTO ValidSampleDTO() 
            => new BankDepositDTO
            {
                Id          = 123,
                Amount      = 456,
                BankAccount = new BankAccountDTO(),
                DocumentRef = "abc"
            };
    }
}
