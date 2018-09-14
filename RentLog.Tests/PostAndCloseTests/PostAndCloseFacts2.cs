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
            var cashierArg = GetTempSampleArgs("Cashier");
            var cashierVm  = new MainWindowVM(29.June(2018), cashierArg, false);
            var suprvsrArg = GetTempSampleArgs("Supervisor", "Mr. Supervisor");
            suprvsrArg.Credentials.HumanName.Should().Be("Mr. Supervisor");
            var suprvsrVm  = new MainWindowVM(29.June(2018), suprvsrArg, false);

            cashierArg.Collections.LastPostedDate().Should().Be(28.June(2018));
            cashierArg.Collections.UnclosedDate().Should().Be(29.June(2018));
            await cashierVm.RefreshCmd.RunAsync();
            cashierVm.PostAndClose.CanPostAndClose().Should().BeFalse();

            var crud = cashierVm.BankDeposits.Crud;
            await crud.SetupForInsert();
            crud.CanSave().Should().BeFalse();
            crud.Draft.BankAccount = crud.BankAccounts[0];
            crud.Draft.Amount = 17307;
            crud.Draft.DocumentRef = "dep slip #";
            crud.CanSave().Should().BeTrue();
            await crud.SaveDraftCmd.RunAsync();

            await cashierVm.RefreshCmd.RunAsync();
            cashierVm.PostAndClose.UpdateTotals();
            cashierVm.PostAndClose.IsBalanced.Should().BeTrue
                (cashierVm.PostAndClose.TotalDifference.ToString());
            cashierVm.PostAndClose.CanPostAndClose().Should().BeFalse();

            var dudes = cashierVm.SectionTabs[0].IntendedColxns.Collectors;
            cashierVm.SectionTabs[0].IntendedColxns.CurrentCollector = dudes[0];
            cashierVm.SectionTabs[1].IntendedColxns.CurrentCollector = dudes[0];
            cashierVm.SectionTabs[2].IntendedColxns.CurrentCollector = dudes[0];

            await suprvsrVm.RefreshCmd.RunAsync();
            suprvsrVm.PostAndClose.CanPostAndClose().Should().BeFalse();

            cashierVm.ApprovalAwaiter.SubmitForApproval();

            await suprvsrVm.RefreshCmd.RunAsync();
            suprvsrVm.PostAndClose.CanPostAndClose().Should().BeTrue
                (suprvsrVm.PostAndClose.PostAndCloseCmd.CurrentLabel);
            suprvsrVm.PostAndClose.RunPostAndClose();

            await Task.Delay(1000 * 5);
            cashierVm  = null;
            cashierArg = null;
            cashierArg = GetTempSampleArgs("Cashier");
            cashierArg.Collections.LastPostedDate().Should().Be(29.June(2018));
            cashierArg.Collections.UnclosedDate().Should().Be(30.June(2018));
            var postdDB = cashierArg.Collections.For(29.June(2018));
            postdDB.IsPosted().Should().BeTrue();
            //postdDB.PostedBy().Should().Be("Mr. Supervisor");

            cashierVm  = new MainWindowVM(30.June(2018), cashierArg, false);
            cashierVm.ColxnsDB.IsOpened().Should().BeFalse();
            await cashierVm.RefreshCmd.RunAsync();
            await Task.Delay(1000 * 3);

            cashierVm.ColxnsDB.IsOpened().Should().BeTrue();
            cashierArg.Balances.TotalOverdues().Rent.Should().BeApproximately(200, 0.02M);
            cashierArg.Balances.TotalOverdues().Rights.Should().Be(0M);

            var rows = cashierArg.Passbooks.GetRepo(1).RowsFor(DateTime.Now.AddDays(-1));
            rows.Should().HaveCount(1);
            rows.Sum(_ => _.Amount).Should().Be(17307);
            rows.Last().RunningBalance.Should().Be(17307);

            rows = cashierArg.Passbooks.GetRepo(2).RowsFor(DateTime.Now.AddDays(-1));
            rows.Should().HaveCount(0);
        }
    }
}
