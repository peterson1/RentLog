using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.Tests.TestTools;
using System;
using Xunit;

namespace RentLog.Tests.StateTransitionTests
{
    [Trait("Deactivate Lease", "Solitary")]
    public class LeaseDeactivatorFacts
    {
        [Fact(DisplayName = "Reject InactiveLease type")]
        public void RejectInactiveLeasetype()
        {
            var sut = new MockMarketState();
            var lse = new InactiveLeaseDTO();
            sut.Invoking(_ => _.DeactivateLease(lse, "", DateTime.Now))
                .Should().Throw<InvalidStateException>();
        }


        [Fact(DisplayName = "Returns record with same Id")]
        public void ReturnsrecordwithsameId()
        {
            var sut = new MockMarketState();
            var lse = new LeaseDTO { Id = 12345 };
            var rec = sut.DeactivateLease(lse, "", DateTime.Now);
            rec.Should().NotBeNull();
            rec.Id.Should().Be(lse.Id);
        }
    }
}
