using RentLog.Tests.TestTools;
using RentLog.DomainLib11.StateTransitions;
using Xunit;
using RentLog.DomainLib11.DTOs;
using FluentAssertions;
using System;
using CommonTools.Lib11.ExceptionTools;
using Moq;

namespace RentLog.Tests.StateTransitionTests
{
    //todo: move all theses to inactives repo tests
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


        //[Fact(DisplayName = "Rejects non-existent Lease record")]
        //public void RejectsnonexistentLeaserecord()
        //{
        //    var sut = new MockDB();
        //    var lse = new LeaseDTO();

        //    sut.MoqActiveLeases.Setup(_ 
        //        => _.HasId(lse.Id)).Returns(false);

        //    sut.Invoking(_ => _.DeactivateLease(lse, "", DateTime.Now))
        //        .Should().Throw<InvalidInsertionException>();
        //}


        [Fact(DisplayName = "Error if record undeleted from Actives")]
        public void ErrorifrecordundeletedforActives()
        {
            var sut = new MockDB();
            var lse = new LeaseDTO();

            sut.MoqActiveLeases.Setup(_ 
                => _.HasId(lse.Id)).Returns(true);

            sut.Invoking(_ => _.DeactivateLease(lse, "", DateTime.Now))
                .Should().Throw<InvalidStateException>();
        }


        [Fact(DisplayName = "Returns record with same Id")]
        public void ReturnsrecordwithsameId()
        {
            var sut = new MockDB();
            var lse = new LeaseDTO();
            var rec = sut.DeactivateLease(lse, "", DateTime.Now);
            rec.Should().NotBeNull();
            rec.Id.Should().Be(lse.Id);
        }
    }
}
