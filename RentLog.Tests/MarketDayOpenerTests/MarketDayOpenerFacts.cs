using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.MarketDayOpenerTests
{
    //[Trait("Market Day Open", "Temp Copy")]
    public class MarketDayOpenerFacts : TempCopyTestBase
    {
        protected override string SampleDirName 
            => SampleDir.JUN17_BALANCED;


        //[Fact(DisplayName = "Day Open Jun 18")]
        public async Task TestMethod00002()
        {
            await RunPostAndClose("Supervisor");

            var arg = GetTempSampleArgs("Cashier");
            arg.Collections.LastPostedDate().Should().Be(17.June(2018));
            arg.Collections.UnclosedDate().Should().Be(18.June(2018));
            var vm  = new MainWindowVM(18.June(2018), arg, false);

            await vm.RefreshCmd.RunAsync();

            var db = arg.Collections.For(18.June(2018));
            db.SectionsSnapshot.Should().NotBeNull();
            db.SectionsSnapshot.Should().HaveCount(3);
        }


        private async Task RunPostAndClose(string role)
        {
            var arg = GetTempSampleArgs(role);
            var vm  = new MainWindowVM(17.June(2018), arg, false);

            await vm.RefreshCmd.RunAsync();
            vm.PostAndClose.CanPostAndClose().Should().BeTrue();

            vm.PostAndClose.RunPostAndClose();
        }
    }
}
