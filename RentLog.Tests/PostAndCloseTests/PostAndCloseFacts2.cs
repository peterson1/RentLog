using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.PostAndCloseTests
{
    [Trait("Post&Close", "Temp Copy")]
    public class PostAndCloseFacts2 : TempCopyTestBase
    {
        protected override string SampleDirName 
            => SampleDir.JUN29_F_MEY;


        [Fact(DisplayName = "Post&Close June 29")]
        public async Task TestMethod00001()
        {
            var arg = GetTempSampleArgs("Cashier");
            var vm = new MainWindowVM(29.June(2018), arg, false);

            await vm.RefreshCmd.RunAsync();
            vm.PostAndClose.CanPostAndClose().Should().BeFalse();

            var crud = vm.BankDeposits.Crud;
            await crud.SetupForInsert();
            crud.CanSave().Should().BeFalse();
            crud.Draft.BankAccount = crud.BankAccounts[0];
            crud.Draft.Amount = 17307;
            crud.Draft.DocumentRef = "dep slip #";
            crud.CanSave().Should().BeTrue();
            await crud.SaveDraftCmd.RunAsync();

            await vm.RefreshCmd.RunAsync();
            vm.PostAndClose.UpdateTotals();
            vm.PostAndClose.IsBalanced.Should().BeTrue(vm.PostAndClose.TotalDifference.ToString());
            vm.PostAndClose.CanPostAndClose().Should().BeFalse();

            var dudes = vm.SectionTabs[0].IntendedColxns.Collectors;
            vm.SectionTabs[0].IntendedColxns.CurrentCollector = dudes[0];
            vm.SectionTabs[1].IntendedColxns.CurrentCollector = dudes[0];
            vm.SectionTabs[2].IntendedColxns.CurrentCollector = dudes[0];


            vm = null;
            arg = null;
            arg = GetTempSampleArgs("Supervisor");
            vm = new MainWindowVM(29.June(2018), arg, false);
            await vm.RefreshCmd.RunAsync();
            vm.PostAndClose.CanPostAndClose().Should().BeTrue(vm.PostAndClose.TotalDifference.ToString());
            await vm.PostAndClose.RunPostAndClose();

            vm = null;
            arg = null;
            arg = GetTempSampleArgs("Cashier");
            arg.Collections.LastPostedDate().Should().Be(29.June(2018));
            arg.Collections.UnclosedDate().Should().Be(30.June(2018));

            vm = new MainWindowVM(30.June(2018), arg, false);
            vm.ShouldClose.Should().BeFalse();
            vm.ColxnsDB.IsOpened().Should().BeFalse();

            await vm.RefreshCmd.RunAsync();

            vm.ColxnsDB.IsOpened().Should().BeTrue();
            arg.Balances.TotalOverdues().Rent.Should().BeApproximately(0M, 0.02M);
            arg.Balances.TotalOverdues().Rights.Should().Be(0M);

            var rows = arg.Passbooks.GetRepo(1).RowsFor(DateTime.Now.AddDays(-1));
            rows.Should().HaveCount(1);
            rows.Sum(_ => _.Amount).Should().Be(17307);
            rows.Last().RunningBalance.Should().Be(17307);

            rows = arg.Passbooks.GetRepo(2).RowsFor(DateTime.Now.AddDays(-1));
            rows.Should().HaveCount(0);

            vm  = null;
            arg = null;
        }
    }
}
