using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using Xunit;

namespace RentLog.Tests.StateTransitionTests
{
    [Trait("Lease IsActive", "Solitary")]
    public class LeaseIsActiveFacts
    {
        [Fact(DisplayName = "Active w/in contract period")]
        public void SimplyActive()
        {
            var sut = SampleActive(2.May(2018), 
                                  29.May(2018));

            sut.IsActive(1.May(2018)).Should().BeFalse();
            sut.IsActive(2.May(2018)).Should().BeTrue();
            sut.IsActive(3.May(2018)).Should().BeTrue();
            sut.IsActive(29.May(2018)).Should().BeTrue();
            sut.IsActive(30.May(2018)).Should().BeFalse();
        }


        [Fact(DisplayName = "Inactive if stall not operational")]
        public void Inactiveifstallnotoperational()
        {
            var sut = SampleActive(2.May(2018),
                                  29.May(2018));

            sut.Stall.IsOperational = false;
            sut.IsActive(3.May(2018)).Should().BeFalse();
        }


        [Fact(DisplayName = "Inactive with deactivation")]
        public void Inactiveafterdeactivation()
        {
            var sut = SampleInactive(2.May(2018),
                                    29.May(2018),
                                    15.May(2018));

            sut.IsActive( 1.May(2018)).Should().BeFalse();
            sut.IsActive( 2.May(2018)).Should().BeTrue();
            sut.IsActive( 3.May(2018)).Should().BeTrue();

            sut.IsActive(14.May(2018)).Should().BeTrue();
            sut.IsActive(15.May(2018)).Should().BeTrue();
            sut.IsActive(16.May(2018)).Should().BeFalse();

            sut.IsActive(29.May(2018)).Should().BeFalse();
            sut.IsActive(30.May(2018)).Should().BeFalse();
        }


        private LeaseDTO SampleActive(DateTime start, DateTime end) => new LeaseDTO
        {
            ContractStart = start,
            ContractEnd   = end,
            Stall         = new StallDTO { IsOperational = true }
        };


        private InactiveLeaseDTO SampleInactive(DateTime start, DateTime end, DateTime deactivation) => new InactiveLeaseDTO
        {
            ContractStart   = start,
            ContractEnd     = end,
            Stall           = new StallDTO { IsOperational = true },
            DeactivatedDate = deactivation
        };
    }
}
