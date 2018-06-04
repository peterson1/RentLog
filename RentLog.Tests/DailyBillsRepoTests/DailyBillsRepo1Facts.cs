using CommonTools.Lib11.DatabaseTools;
using FluentAssertions.Extensions;
using Moq;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using Xunit;

namespace RentLog.Tests.DailyBillsRepoTests
{
    public class DailyBillsRepo1Facts
    {
        //[Fact(DisplayName = "Opens next day")]
        //public void OpensNextDay()
        //{
        //    var lse     = new LeaseDTO();
        //    var bill    = new Mock<ISimpleRepo<DailyBillDTO>>();
        //    var colxn   = new Mock<ICollectionsDir>();
        //    var sut     = new DailyBillsRepo1(lse, bill.Object, colxn.Object);
        //    var unclosd = 2.May(2018);

        //    sut.OpenNextDay(unclosd);

        //    //sut.
        //}
    }
}
