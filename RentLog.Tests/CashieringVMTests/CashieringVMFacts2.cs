using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.CashieringVMTests
{
    [Trait("Cashiering VM", "Temp Copy")]
    public class CashieringVMFacts2 : TempCopyTestBase
    {
        protected override string SampleDirName 
            => SampleDir.JUN17_BALANCED;


        [Fact(DisplayName = "Creates Sections Snapshot")]
        public async Task TestMethod00002()
        {
            var arg = GetTempSampleArgs("Cashier");
            var db = arg.Collections.For(17.June(2018));
            db.SectionsSnapshot.Should().BeNull();

            var vm  = new MainWindowVM(17.June(2018), arg, false);
            await vm.RefreshCmd.RunAsync();

            await vm.OnWindowClosing(new CancelEventArgs());

            arg = null;
            arg = GetTempSampleArgs("Cashier");
            db  = arg.Collections.For(17.June(2018));
            db.SectionsSnapshot.Should().NotBeNull();
            db.SectionsSnapshot.Should().HaveCount(3);
        }
    }
}
