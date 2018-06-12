using FluentAssertions;
using RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using Xunit;
using static RentLog.DomainLib11.DTOs.FundRequestDTO;

namespace RentLog.Tests.AllocationsListVMTests
{
    [Trait("Allocations List VM", "Solitary")]
    public class AllocationsListVMFacts
    {
        [Fact(DisplayName = "SetHost clears list")]
        public void SetHostclearslist()
        {
            var sut  = new AllocationsListVM();
            var list = new List<AccountAllocation>
            {
                new AccountAllocation(),
                new AccountAllocation(),
                new AccountAllocation(),
            };

            sut.Add(new AccountAllocation());
            sut.Add(new AccountAllocation());

            sut.SetHost(list, null);

            sut.Should().HaveCount(3);
        }


        [Fact(DisplayName = "Empty list, valid amount: creates Cash-in-bank entry")]
        public void OnvalidamountenterecreatesCashinbankentry()
        {
            var sut  = new AllocationsListVM();
            var host = new List<AccountAllocation>();
            var bank = BankAccountDTO.Named("test bank acct");
            var amt  = 123;
            sut.SetHost(host, bank);

            sut.OnAmountChanged(amt);

            sut.Should().HaveCount(1);
            sut[0].Account.Name.Should().Contain("Cash in Bank");
            sut[0].Account.Name.Should().Contain(bank.Name);
            sut[0].SubAmount.Should().Be(amt);
        }


        [Fact(DisplayName = "Has Item, amount changed: updates item", Skip ="undone")]
        public void HasItemamountchangedupdatesitem()
        {

        }
    }
}
