using FluentAssertions.Extensions;
using Moq;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using Xunit;

namespace RentLog.Tests.BalAdjsRepoTests
{
    [Trait("Bal Adjs. Repo", "Solitary")]
    public class BalAdjsRepoFacts
    {
        [Fact(DisplayName = "Insert calls BatchBalUpdate")]
        public void InsertcallsBatchBalUpdate()
        {
            var mColxnsDir  = new Mock<ICollectionsDir>();
            var mDailyBilr  = new Mock<IDailyBiller>();
            var mBillsRepo  = new Mock<IDailyBillsRepo>();
            var mBalsDB     = new Mock<IBalanceDB>();
            var mBalRepo    = new Mock<IBalanceAdjustmentsRepo>();
            mBalsDB.Setup(_ => _.GetRepo(It.IsAny<int>())).Returns(mBillsRepo.Object);
            var date = 3.May(2018);
            var sut = new BalanceAdjsRepo1(date, mBalRepo.Object, mBalsDB.Object, mColxnsDir.Object);
            sut.Insert(new BalanceAdjustmentDTO());
            mBillsRepo.Verify(_
                => _.UpdateFrom(date, It.IsAny<BillCode>(), It.IsAny<IDailyBiller>()), 
                        Times.Once());
        }


        [Fact(DisplayName = "Update calls BatchBalUpdate")]
        public void UpdatecallsBatchBalUpdate()
        {
            var mColxnsDir  = new Mock<ICollectionsDir>();
            var mDailyBilr  = new Mock<IDailyBiller>();
            var mBillsRepo  = new Mock<IDailyBillsRepo>();
            var mBalsDB     = new Mock<IBalanceDB>();
            var mBalRepo    = new Mock<IBalanceAdjustmentsRepo>();
            mBalsDB.Setup(_ => _.GetRepo(It.IsAny<int>())).Returns(mBillsRepo.Object);
            var date = 3.May(2018);
            var sut = new BalanceAdjsRepo1(date, mBalRepo.Object, mBalsDB.Object, mColxnsDir.Object);
            sut.Update(new BalanceAdjustmentDTO());
            mBillsRepo.Verify(_
                => _.UpdateFrom(date, It.IsAny<BillCode>(), It.IsAny<IDailyBiller>()), Times.Once());
        }


        [Fact(DisplayName = "Delete calls BatchBalUpdate")]
        public void DeletecallsBatchBalUpdate()
        {
            var mColxnsDir  = new Mock<ICollectionsDir>();
            var mDailyBilr  = new Mock<IDailyBiller>();
            var mBillsRepo  = new Mock<IDailyBillsRepo>();
            var mBalsDB     = new Mock<IBalanceDB>();
            var mBalRepo    = new Mock<IBalanceAdjustmentsRepo>();
            mBalsDB.Setup(_ => _.GetRepo(It.IsAny<int>())).Returns(mBillsRepo.Object);
            var date = 3.May(2018);
            var sut = new BalanceAdjsRepo1(date, mBalRepo.Object, mBalsDB.Object, mColxnsDir.Object);
            sut.Delete(new BalanceAdjustmentDTO());
            mBillsRepo.Verify(_
                => _.UpdateFrom(date, It.IsAny<BillCode>(), It.IsAny<IDailyBiller>()), Times.Once());
        }
    }
}
