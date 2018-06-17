using FluentAssertions;
using RentLog.ChequeVouchers.VoucherReqsTab.FundRequests;
using RentLog.ChequeVouchers.VoucherReqsTab.IssuedCheques;
using RentLog.ChequeVouchers.VoucherReqsTab.PreparedCheques;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.VoucherTabVMTests
{
    [Trait("Voucher Tab VMs", "Sample Dir")]
    public class FundReqsListVMFacts
    {
        [Fact(DisplayName = "Fund Requests List")]
        public void FundRequestsList()
        {
            var dir = SampleDir.May19_GRY();
            var sut = new FundReqsListVM(dir);
            sut.ReloadFromDB();
            sut.ItemsList.Count.Should().Be(17);
            sut.TotalSum.Should().Be(501_380.77M);
        }


        [Fact(DisplayName = "Prepared Cheques List")]
        public void PreparedChequesList()
        {
            var dir = SampleDir.May19_GRY();
            var sut = new PreparedChequesListVM(dir);
            sut.ReloadFromDB();
            sut.ItemsList.Count.Should().Be(9);
            sut.TotalSum.Should().Be(318_275.63M);
        }


        [Fact(DisplayName = "Issued Cheques List")]
        public void IssuedChequesList()
        {
            var dir = SampleDir.May19_GRY();
            var sut = new IssuedChequesListVM(dir);
            sut.ReloadFromDB();
            sut.ItemsList.Count.Should().Be(175);
            sut.TotalSum.Should().Be(3_296_842.70M);
        }
    }
}
