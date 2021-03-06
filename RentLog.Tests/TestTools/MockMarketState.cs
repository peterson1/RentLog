﻿using Moq;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.Tests.TestTools
{
    class MockMarketState : MarketStateDbBase
    {
        public MockMarketState()
        {
            MoqStalls         = new Mock<IStallsRepo>();
            MoqSections       = new Mock<ISectionsRepo>();
            MoqBankAccounts   = new Mock<IBankAccountsRepo>();
            MoqActiveLeases   = new Mock<IActiveLeasesRepo>();
            MoqInactiveLeases = new Mock<IInactiveLeasesRepo>();
            MoqBalanceDB      = new Mock<IBalanceDB>();
        }

        public Mock<IStallsRepo>          MoqStalls         { get; }
        public Mock<ISectionsRepo>        MoqSections       { get; }
        public Mock<IBankAccountsRepo>    MoqBankAccounts   { get; }
        public Mock<IActiveLeasesRepo>    MoqActiveLeases   { get; }
        public Mock<IInactiveLeasesRepo>  MoqInactiveLeases { get; }
        public Mock<IBalanceDB>           MoqBalanceDB      { get; }


        public override IStallsRepo         Stalls         => MoqStalls.Object;
        public override ISectionsRepo       Sections       => MoqSections.Object;
        public override IActiveLeasesRepo   ActiveLeases   => MoqActiveLeases.Object;
        public override IInactiveLeasesRepo InactiveLeases => MoqInactiveLeases.Object;
        public override IBalanceDB          Balances       => MoqBalanceDB.Object;
        public override IBankAccountsRepo   BankAccounts   => MoqBankAccounts.Object;
        public override int                 YearsBackCount { get; set; }
    }
}
