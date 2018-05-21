using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.Tests.SampleDBs;
using System;
using Xunit;

namespace RentLog.Tests.TotalOverduesTests
{
    [Trait("Total Overdues", "Sample DB Dir")]
    public class TotalOverduesFacts
    {
        [Fact(DisplayName = "May 19 Total Overdues")]
        public void May19TotalOverdues()
        {
            var dir = SampleDir.May_19_F_Garay();


            //foreach (var lse in dir.MarketState.ActiveLeases.GetAll())
            //{
            //    var repo = dir.Balances.GetRepo(lse);
            //}

            var bal = dir.Balances.TotalOverdues(19.May(2018));
            bal.Rent.Should().BeApproximately(223_540.52M, 0.1M);
            bal.Rights.Should().Be(0M);
        }
    }
}
