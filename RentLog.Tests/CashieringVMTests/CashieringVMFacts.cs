using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.Tests.SampleDBs;
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

            sut.BankDeposits.Should().HaveCount(2);
            sut.BankDeposits.TotalSum.Should().Be(15_831);

            sut.OtherColxns.Should().HaveCount(1);
            sut.OtherColxns.TotalSum.Should().Be(416);

            sut.CashierColxns.Should().HaveCount(0);

            sut.SectionTabs[0].IntendedColxns.Should().HaveCount(31);
            sut.SectionTabs[0].IntendedColxns.TotalSum.Should().Be(6_890);
            sut.SectionTabs[0].AmbulantColxns.Should().HaveCount(0);
            sut.SectionTabs[0].AmbulantColxns.TotalSum.Should().Be(0);
            sut.SectionTabs[0].Uncollecteds.Should().HaveCount(4);
            sut.SectionTabs[0].NoOperations.Should().HaveCount(13);

            //sut.TotalCollections.Should().Be(15_831);
            sut.TotalDeposits.Should().Be(15_831);
        }


        [Fact(DisplayName = "May 8")]
        public async Task May8()
        {
            var args = SampleArgs.HelenAblen_Dry8();
            var sut = new MainWindowVM(8.May(2018), args, false);
            await sut.RefreshCmd.RunAsync();

            sut.BankDeposits.Should().HaveCount(3);
            sut.BankDeposits.TotalSum.Should().Be(23_298);

            sut.OtherColxns.Should().HaveCount(1);
            sut.OtherColxns.TotalSum.Should().Be(519);

            sut.CashierColxns.Should().HaveCount(1);
            sut.CashierColxns.TotalSum.Should().Be(5_000);
        }
    }
}
