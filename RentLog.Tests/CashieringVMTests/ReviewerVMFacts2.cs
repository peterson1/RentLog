using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.Tests.TestTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.CashieringVMTests
{
    [Trait("Reviewer VM", "Solitary")]
    public class ReviewerVMFacts2
    {
        [Fact(DisplayName = "Reviewing newly closed exits app")]
        public void Reviewingnewlyclosedexitsapp()
        {
            var args = new MockDBsDir();
            var date = 2.May(2018);
            var sut  = new MainWindowVM(date, args);
        }
    }
}
