using FluentAssertions.Extensions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System.Collections.Generic;
using Xunit;

namespace RentLog.Tests.UncollectedsRepoTests
{
    [Trait("Uncollecteds Repo", "Sample DBs")]
    public class UncollectedsRepoFacts2 : TempCopyTestBase
    {
        protected override string SampleDirName => SampleDir.JUL21_NO_UNCOL;


        [Fact(DisplayName = "Can DropAndInsert", Skip = "LiteDB Open Issue")]
        public void TestMethod00001()
        {
            var dir = GetTempSampleArgs("Cashier");
            var db  = dir.Collections.For(21.July(2018));
            var list = new List<UncollectedLeaseDTO>
            {
                ValidSampleDTO(),
            };

            db.Uncollecteds[1].DropAndInsert(list, false);
            db.Uncollecteds[2].DropAndInsert(list, false);
            db.Uncollecteds[3].DropAndInsert(list, false);
        }


        private UncollectedLeaseDTO ValidSampleDTO() => new UncollectedLeaseDTO
        {
            Lease         = new LeaseDTO(),
            StallSnapshot = new StallDTO(),
            Targets       = new BillAmounts()
        };
    }
}
