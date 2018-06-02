using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.DTOs;
using RentLog.Tests.SampleDBs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.CollectionsDirTests
{
    [Trait("Collections Dir", "Sample DBs")]
    public class CollectionsDirFacts
    {
        [Fact(DisplayName = "Unclosed Date")]
        public void UnclosedDate()
        {
            var arg = SampleDir.Lease197(out LeaseDTO lse);
            arg.Collections.UnclosedDate().Should().Be(13.May(2018));
        }


        [Fact(DisplayName = "Last Posted Date")]
        public void LastPostedDate()
        {
            var arg = SampleDir.Lease197(out LeaseDTO lse);
            arg.Collections.LastPostedDate().Should().Be(12.May(2018));
        }
    }
}
