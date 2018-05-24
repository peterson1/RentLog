using RentLog.Tests.TestTools;
using RentLog.DomainLib11.StateTransitions;
using Xunit;
using RentLog.DomainLib11.DTOs;
using FluentAssertions;
using System;
using CommonTools.Lib11.ExceptionTools;
using Moq;
using RentLog.DomainLib11.BalanceRepos;

namespace RentLog.Tests.StateTransitionTests
{
    [Trait("Deactivate Lease", "Solitary")]
    public class LeaseDeactivatorFacts
    {
        [Fact(DisplayName = "Reject InactiveLease type")]
        public void RejectInactiveLeasetype()
        {
            var sut = new MockDB();
            var lse = new InactiveLeaseDTO();
            sut.Invoking(_ => _.DeactivateLease(lse, "", DateTime.Now))
                .Should().Throw<InvalidStateException>();
        }


        [Fact(DisplayName = "Returns record with same Id")]
        public void ReturnsrecordwithsameId()
        {
            var sut = new MockDB();
            var lse = new LeaseDTO { Id = 12345 };
            var rec = sut.DeactivateLease(lse, "", DateTime.Now);
            rec.Should().NotBeNull();
            rec.Id.Should().Be(lse.Id);
        }
    }
}
