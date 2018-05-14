using Moq;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.CollectionRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.DailyBillerTests
{
    [Trait("Rent Biller", "Solitary")]
    public class RentBillerFacts
    {
        [Fact(DisplayName = "Rent surcharge case 1", Skip = "Undone")]
        public void Rentsurchargecase1()
        {
            var moq = new Mock<ICollectionsDB>();
            var sut = new DailyBiller1(moq.Object);

            //sut.ComputeBill()
        }
    }
}
