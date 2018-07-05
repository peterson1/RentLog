using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DateTimeTools;
using FluentAssertions;
using Moq;
using RentLog.DomainLib11.DTOs;
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
        const int OCCUPIED_BY_SAMPLE  = 123;
        const int OCCUPIED_BY_ANOTHER = 456;

        [Fact(DisplayName = "Rejects invalid Id")]
        public void TestMethod00000()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Id = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeTrue(why);
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeFalse();

            obj.Id = 123;
            sut.IsValidForInsert(obj, out why).Should().BeFalse();
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Id = -456;
            sut.IsValidForInsert(obj, out why).Should().BeFalse(why);
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeFalse();
        }


        [Fact(DisplayName = "Rejects NULL tenant")]
        public void TestMethod00001()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant = ValidTenant();
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank First Name")]
        public void TestMethod00003()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.FirstName = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.FirstName = "Some name";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank Middle Name")]
        public void TestMethod00004()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.MiddleName = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.MiddleName = "Some name";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank Last Name")]
        public void TestMethod00005()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.LastName = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.LastName = "Some name";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Invalid Birthdate")]
        public void TestMethod00006()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.BirthDate = DateTime.MinValue;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.BirthDate = DateTime.Now;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank Phone # 1")]
        public void TestMethod00007()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.Phone1 = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.Phone1 = "123";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank Lot #")]
        public void TestMethod00008()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.LotNumber = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.LotNumber = "#13";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank Street name")]
        public void TestMethod00009()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.StreetName = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.StreetName = "#13";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank Barangay")]
        public void TestMethod00010()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.Barangay = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.Barangay = "#13";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank Municipality")]
        public void TestMethod00011()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.Municipality = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.Municipality = "#13";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank Province")]
        public void TestMethod00012()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Tenant.Province = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Tenant.Province = "#13";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Blank ProductToSell")]
        public void TestMethod00013()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.ProductToSell = null;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.ProductToSell = "some product";
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects invalid Contract Start")]
        public void TestMethod00014()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.ContractStart = DateTime.MinValue;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.ContractStart = 2.May(2018);
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects invalid Contract End")]
        public void TestMethod00015()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.ContractEnd = 1.May(2018);
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.ContractEnd = 2.May(2018);
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 123;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        [Fact(DisplayName = "Rejects Occupied Stall")]
        public void TestMethod00016()
        {
            var sut = CreateSUT(out LeaseDTO obj);

            obj.Stall.Id = OCCUPIED_BY_ANOTHER;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out string why).Should().BeFalse();
            obj.Id = 101;
            sut.IsValidForUpdate(obj, out why).Should().BeFalse();
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);

            obj.Stall.Id = OCCUPIED_BY_SAMPLE;
            obj.Id = 0;
            sut.IsValidForInsert(obj, out why).Should().BeTrue(why);
            obj.Id = 101;
            sut.IsValidForUpdate(obj, out why).Should().BeTrue(why);
            sut.IsValidForDelete(obj, out why).Should().BeTrue(why);
        }


        private ActiveLeasesRepo1 CreateSUT(out LeaseDTO validSample)
        {
            var mkt     = new Mock<IMarketStateDB>();
            var moq     = new Mock<ISimpleRepo<LeaseDTO>>();
            var sut     = new ActiveLeasesRepo1(moq.Object, mkt.Object);
            validSample = ValidSampleDTO();

            moq.Setup(_ => _.Any()).Returns(true);

            moq.Setup(_ => _.Find(It.IsAny<int>(), true)).Returns(ValidSampleDTO());

            moq.Setup(_ => _.GetAll())
                .Returns(new List<LeaseDTO>
                {
                    //new LeaseDTO { Stall = new StallDTO{ Id = OCCUPIED_BY_SAMPLE  }},
                    new LeaseDTO { Stall = new StallDTO{ Id = OCCUPIED_BY_ANOTHER }}
                });

            return sut;
        }


        private LeaseDTO ValidSampleDTO() => new LeaseDTO
        {
            Id            = 0,
            Tenant        = ValidTenant(),
            Stall         = new StallDTO { Id = OCCUPIED_BY_SAMPLE },
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
            Province     = "Some Province"
        };
    }
}
