using CommonTools.Lib11.DatabaseTools;
using Moq;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.DailyBillsRepoTests
{
    [Trait("Daily Bills Repo", "Solitary")]
    public class DailyBillsRepoFacts
    {
        [Fact(DisplayName = "blah")]
        public void blah()
        {
            var mRepo = new Mock<ISimpleRepo<DailyBillDTO>>();
            var sut   = new DailyBillsRepo1(mRepo.Object);
            //sut.UpdateFrom()
        }

    }
}
