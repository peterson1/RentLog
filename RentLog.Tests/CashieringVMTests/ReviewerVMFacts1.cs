﻿using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.Tests.SampleDBs;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.CashieringVMTests
{
    [Trait("Reviewer VM", "Sample DBs")]
    public class ReviewerVMFacts1
    {
        [Fact(DisplayName = "Review May 12")]
        public async Task ReviewMay12()
        {
            var args = SampleDir.Lease197();
            var sut  = new MainWindowVM(12.May(2018), args, false);
            await sut.RefreshCmd.RunAsync();

            sut.CashierColxns.Should().HaveCount(0);
            sut.CashierColxns.TotalSum.Should().Be(0);
            sut.OtherColxns.Should().HaveCount(1);
            sut.OtherColxns.TotalSum.Should().Be(416);
            sut.BankDeposits.Should().HaveCount(2);
            sut.BankDeposits.TotalSum.Should().Be(15_831);

            sut.SectionTabs[0].IntendedColxns.Should().HaveCount(31);
            sut.SectionTabs[0].IntendedColxns.TotalSum.Should().Be(6_890);
            sut.SectionTabs[0].AmbulantColxns.Should().HaveCount(0);
            sut.SectionTabs[0].AmbulantColxns.TotalSum.Should().Be(0);
            sut.SectionTabs[0].Uncollecteds.Should().HaveCount(4);
            sut.SectionTabs[0].NoOperations.Should().HaveCount(13);
            sut.SectionTabs[0].SectionTotal.Should().Be(6_890);
            sut.SectionTabs[0].Collector.Name.Should().Be("Jomar Pasaludos");

            sut.SectionTabs[1].IntendedColxns.Should().HaveCount(63);
            sut.SectionTabs[1].IntendedColxns.TotalSum.Should().Be(7_785);
            sut.SectionTabs[1].AmbulantColxns.Should().HaveCount(4);
            sut.SectionTabs[1].AmbulantColxns.TotalSum.Should().Be(530);
            sut.SectionTabs[1].Uncollecteds.Should().HaveCount(2);
            sut.SectionTabs[1].NoOperations.Should().HaveCount(11);
            sut.SectionTabs[1].SectionTotal.Should().Be(8_315);
            sut.SectionTabs[1].Collector.Name.Should().Be("Jomar Pasaludos");

            sut.PostAndClose.TotalCollections.Should().Be(15_621);
            sut.PostAndClose.TotalDeposits.Should().Be(15_831);
            sut.PostAndClose.IsBalanced.Should().BeFalse();
        }


        [Fact(DisplayName = "Review May 8")]
        public async Task ReviewMay8()
        {
            var args = SampleDir.Lease197();
            var sut  = new MainWindowVM(8.May(2018), args, false);
            await sut.RefreshCmd.RunAsync();

            sut.CashierColxns.Should().HaveCount(1);
            sut.CashierColxns.TotalSum.Should().Be(5_000);
            sut.OtherColxns.Should().HaveCount(1);
            sut.OtherColxns.TotalSum.Should().Be(519);
            sut.BankDeposits.Should().HaveCount(3);
            sut.BankDeposits.TotalSum.Should().Be(23_298);

            sut.SectionTabs[0].IntendedColxns.Should().HaveCount(38);
            sut.SectionTabs[0].IntendedColxns.TotalSum.Should().Be(7_466);
            sut.SectionTabs[0].AmbulantColxns.Should().HaveCount(0);
            sut.SectionTabs[0].AmbulantColxns.TotalSum.Should().Be(0);
            sut.SectionTabs[0].Uncollecteds.Should().HaveCount(7);
            sut.SectionTabs[0].NoOperations.Should().HaveCount(3);
            sut.SectionTabs[0].SectionTotal.Should().Be(7_466);

            sut.SectionTabs[1].IntendedColxns.Should().HaveCount(67);
            sut.SectionTabs[1].IntendedColxns.TotalSum.Should().Be(9_560);
            sut.SectionTabs[1].AmbulantColxns.Should().HaveCount(6);
            sut.SectionTabs[1].AmbulantColxns.TotalSum.Should().Be(585);
            sut.SectionTabs[1].Uncollecteds.Should().HaveCount(0);
            sut.SectionTabs[1].NoOperations.Should().HaveCount(9);
            sut.SectionTabs[1].SectionTotal.Should().Be(10_145);

            sut.PostAndClose.TotalCollections.Should().Be(23_130);
            sut.PostAndClose.TotalDeposits.Should().Be(23_298);
            sut.PostAndClose.IsBalanced.Should().BeFalse();
        }
    }
}
