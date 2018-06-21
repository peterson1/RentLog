using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System.Linq;
using Xunit;

namespace RentLog.Tests.RecomputeRunningBalTests
{
    [Trait("Recompute Running Bals", "Temp Copy")]
    public class RecomputeRunningBalFacts : TempCopyTestBase
    {
        protected override string SampleDirName
            => SampleDir.JUN17_BALANCED;


        [Fact(DisplayName = "Recompute RunningBal")]
        public void TestMethod00002()
        {
            var arg = GetTempSampleArgs();

            var sut = arg.Passbooks.GetRepo(1);
            var row = sut.RowsFor(1.June(2018)).Single();
            row.RunningBalance.Should().Be(-13_704_366.56M);

            sut.RecomputeBalancesFrom(3.June(2018));

            row = sut.RowsFor(1.June(2018)).Single();
            row.RunningBalance.Should().Be(-13_704_366.56M);
        }
    }
}
