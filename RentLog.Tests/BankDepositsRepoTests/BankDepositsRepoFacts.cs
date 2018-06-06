using CommonTools.Lib11.DatabaseTools;
using Moq;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using Xunit;

namespace RentLog.Tests.BankDepositsRepoTests
{
    public class BankDepositsRepoFacts
    {
        [Fact(DisplayName = "Invalid if 0 amount")]
        public void Invalidif0amount()
        {
            var moq = new Mock<ISimpleRepo<BankDepositDTO>>();
            var sut = new BankDepositsRepo1(moq.Object);

            var obj = new BankDepositDTO
            {
                Amount = 0
            };

            //sut.IsValidForInsert(obj)
        }

        //todo: test zero Id for update
    }
}
