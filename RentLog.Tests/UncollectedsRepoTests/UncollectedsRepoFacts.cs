﻿using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.DomainLib11.Models;
using RentLog.Tests.SampleDBs;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.UncollectedsRepoTests
{
    [Trait("Uncollecteds Repo", "Sample DBs")]
    public class UncollectedsRepoFacts
    {
        private const int WET = 1;
        private const int DRY = 2;
        private const int FRZ = 3;

        [Fact(DisplayName = "GetDue: With Backrent")]
        public void TestMethod00001()
        {
            var arg = SampleDir.Jun16_GRY();
            var lse = arg.MarketState.ActiveLeases.Find(158, true);
            var sut = arg.Collections.For(16.June(2018)).Uncollecteds[DRY];
            sut.GetDue(lse, BillCode.Rent).Should().Be(7800 + 234 + 167);
        }


        [Fact(DisplayName = "GetDue: No Backrent")]
        public void TestMethod00002()
        {
            var arg = SampleDir.Jun16_GRY();
            var lse = arg.MarketState.ActiveLeases.Find(78, true);
            var sut = arg.Collections.For(16.June(2018)).Uncollecteds[WET];
            sut.GetDue(lse, BillCode.Rent).Should().Be(0 + 80);
        }


        [Fact(DisplayName = "GetDue: Overpaid")]
        public void TestMethod00003()
        {
            var arg = SampleDir.Jun16_GRY();
            var lse = arg.MarketState.ActiveLeases.Find(81, true);
            var sut = arg.Collections.For(16.June(2018)).Uncollecteds[WET];
            sut.GetDue(lse, BillCode.Rent).Should().Be(-191 + 90);
        }


        [Fact(DisplayName = "Infer Uncollecteds")]
        public async Task TestMethod00004()
        {
            var arg = SampleDir.Jun16_GRY();
            arg.Credentials.Roles = "Cashier";
            var dte = arg.Collections.UnclosedDate();
            var win = new MainWindowVM(dte, arg, false);
            await win.RefreshCmd.RunAsync();
            win.SectionTabs[0].Uncollecteds.Should().HaveCount(6);
            win.SectionTabs[1].Uncollecteds.Should().HaveCount(0);
            win.SectionTabs[2].Uncollecteds.Should().HaveCount(3);
        }
    }
}