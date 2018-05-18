using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.Tests.SampleDBs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RentLog.Tests.BillRowComposerTests
{
    [Trait("Rent Bill Composer 1", "Solitary")]
    public class RentBillComposerFacts
    {
        //[Fact(DisplayName = "Closing: negative bal with payment", Skip = "Undone")]
        //public void Closingbalancenopenalty()
        //{
        //    var arg = SampleDir.HelenAblen_Dry8(out LeaseDTO lse);
        //    var sut = new RentBillComposer1(arg.Collections);
        //    var row = arg.Balances.GetBill(lse, 5.May(2018));
        //    var bil = row.For(BillCode.Rent);
        //    sut.ComputeClosingBalance(bil).Should().Be(-655);
        //}

        [Fact(DisplayName = "Normal case 1")]
        public void TestMethod()
        {

        }


        //todo: test inactive lease
        //todo: test grace period
    }
}
