using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DateTimeTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.JournalVoucherRepos;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace RentLog.Tests.LeasesTests
{
    [Trait("Active Leases Repo", "Solitary")]
    public class ActiveLeasesRepoValidationFacts
    {
        [Fact(DisplayName = "Rejects invalid Id")]
        public void TestMethod00000()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Id = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeFalse();

            obj.Id = 123;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Id = -456;
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeFalse();
        }


        [Fact(DisplayName = "Rejects NULL tenant")]
        public void TestMethod00001()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Tenant = ValidTenant();
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        [Fact(DisplayName = "Rejects Blank First Name")]
        public void TestMethod00003()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.FirstName = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();

            obj.Tenant.FirstName = "Some name";
            sut.IsValidForInsert(obj, out why).Should().BeTrue();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue();
            sut.IsValidForDelete(obj, out why).Should().BeTrue();
        }


        private ActiveLeasesRepo1 CreateSUT(out LeaseDTO validSample)
        {
            var mkt     = new Mock<IMarketStateDB>();
            var moq     = new Mock<ISimpleRepo<LeaseDTO>>();
            var sut     = new ActiveLeasesRepo1(moq.Object, mkt.Object);
            validSample = ValidSampleDTO();
            return sut;
        }


        private LeaseDTO ValidSampleDTO() => new LeaseDTO
        {
            Id            = 101,
            Tenant        = ValidTenant(),
            Stall         = new StallDTO { Id = 123 },
            ContractStart = 2.May(2018),
            ContractEnd   = 1.May(2019),
            ProductToSell = "some product"
        };


        private TenantModel ValidTenant() => new TenantModel
        {
            FirstName    = "first name",
            MiddleName   = "middle name",
            LastName     = "last name",
            BirthDate    = 7.Feb(2014),
            Phone1       = "(02) 123-4567",
            LotNumber    = "#123",
            StreetName   = "Any Street",
            Barangay     = "Brgy. Anywhere",
            Municipality = "Some City",
            Country      = "Philippines"
        };
    }
}
