using CommonTools.Lib11.DatabaseTools;
using Moq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.PassbookRowsRepoTests
{
    public class PassbookRowsRepoFacts
    {
        [Fact(DisplayName = "Rejects invalid Bank Acct ID")]
        public void RejectinvalidBankAcctID()
        {
            var acctID = 1234;
            var moq    = new Mock<ISimpleRepo<PassbookRowDTO>>();
            var sut    = new TestClass(acctID);

            //sut.IsValidForInsert()
        }


        private class TestClass : VagrantRepoFacadeBase
        {
            public TestClass(int bankAccountId) : base(bankAccountId)
            {
            }

            protected override ISimpleRepo<PassbookRowDTO> FindRepo(DateTime date)
            {
                throw new NotImplementedException();
            }
        }
    }
}
