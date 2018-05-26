using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.DomainLib11.DTOs;
using RentLog.Tests.SampleDBs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.CashieringVMTests
{
    [Trait("Cashiering VM", "Sample DBs")]
    public class CashieringVMFacts
    {
        [Fact(DisplayName = "May 12")]
        public async Task May12()
        {
            var args = SampleArgs.HelenAblen_Dry8();
            var sut  = new MainWindowVM(12.May(2018), args, false);
            await sut.RefreshCmd.RunAsync();

            sut.BankDeposits.ItemsList.Should().HaveCount(2);
            sut.BankDeposits.TotalSum.Should().Be(15_831);

            sut.OtherColxns.ItemsList.Should().HaveCount(1);
            sut.OtherColxns.TotalSum.Should().Be(416);

            sut.CashierColxns.ItemsList.Should().HaveCount(0);
        }


        [Fact(DisplayName = "May 8")]
        public async Task May8()
        {
            var args = SampleArgs.HelenAblen_Dry8();
            var sut = new MainWindowVM(8.May(2018), args, false);
            await sut.RefreshCmd.RunAsync();

            //sut.SectionTabs[0].

            sut.BankDeposits.ItemsList.Should().HaveCount(3);
            sut.BankDeposits.TotalSum.Should().Be(23_298);

            sut.OtherColxns.ItemsList.Should().HaveCount(1);
            sut.OtherColxns.TotalSum.Should().Be(519);

            sut.CashierColxns.ItemsList.Should().HaveCount(1);
            sut.CashierColxns.TotalSum.Should().Be(5_000);
        }
    }
}
