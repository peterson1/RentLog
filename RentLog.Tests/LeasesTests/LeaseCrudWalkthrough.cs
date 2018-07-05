using FluentAssertions;
using RentLog.DomainLib45.StallPicker;
using RentLog.LeasesCrud;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.LeasesTests
{
    [Trait("Lease Crud Walkthrough", "Temp Copy")]
    public class LeaseCrudWalkthrough : TempCopyTestBase
    {
        protected override string SampleDirName => SampleDir.JUN17_BALANCED;


        [Fact(DisplayName = "Insert new Lease")]
        public async Task TestMethod00001()
        {
            var arg   = GetTempSampleArgs("Supervisor");
            var main  = new MainWindowVM(arg, false);
            var rows  = main.ActiveLeases.Rows;
            var crud  = main.ActiveLeases.Crud;
            var stall = StallPicker.PickFirstVacant(arg.MarketState);
            stall.DefaultRent.RegularRate.Should().Be(160);

            await main.RefreshCmd.RunAsync();
            rows.Should().HaveCount(113);
            crud.SetPickedStall(stall);

            await crud.SetupForInsert();
            crud.CanSave().Should().BeFalse();
            crud.Draft.ProductToSell = "something";
            crud.Draft.Tenant.FirstName = "first name";
            crud.Draft.Tenant.MiddleName = "middle name";
            crud.Draft.Tenant.LastName = "last name";
            crud.DraftBirthDate = DateTime.Now.AddYears(-30);
            crud.Draft.Tenant.Phone1 = "123-4567";
            crud.Draft.Tenant.LotNumber = "#13";
            crud.Draft.Tenant.StreetName = "street";
            crud.Draft.Tenant.Barangay = "brgy";
            crud.Draft.Tenant.Municipality = "muni";
            crud.Draft.Tenant.Province = "prov";
            crud.Draft.Rent.RegularRate = 200;
            crud.CanSave().Should().BeTrue(crud.WhyInvalid);

            await crud.SaveDraftCmd.RunAsync();
            await main.RefreshCmd.RunAsync();
            rows.Should().HaveCount(114);
            rows.First().DTO.Stall.Id  .Should().Be(stall.Id);
            rows.First().DTO.Stall.Name.Should().Be(stall.Name);
            arg.MarketState.Stalls.Find(stall.Id, true)
                .DefaultRent.RegularRate.Should().Be(200);
        }


        [Fact(DisplayName = "Edit Lease")]
        public async Task TestMethod00002()
        {
            var arg   = GetTempSampleArgs("Supervisor");
            var main  = new MainWindowVM(arg, false);
            var rows  = main.ActiveLeases.Rows;
            var crud  = main.ActiveLeases.Crud;

            await main.RefreshCmd.RunAsync();
            rows.Should().HaveCount(113);

            crud.SetupForUpdate(rows[0].DTO);

            crud.CanSave().Should().BeTrue(crud.WhyInvalid);
        }
    }
}
