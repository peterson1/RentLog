using CommonTools.Lib11.GoogleTools;
using Moq;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.PassbookRepos;

namespace RentLog.Tests.TestTools
{
    class MockDBsDir : ITenantDBsDir
    {
        public Mock<ICollectionsDir> MoqCollectionsDir { get; } = new Mock<ICollectionsDir>();
        public Mock<IBalanceDB>      MoqBalanceDB      { get; } = new Mock<IBalanceDB>();
        public MockMarketState       MoqMarketState    { get; } = new MockMarketState();
        public Mock<IPassbookDB>     MoqPassbookDB     { get; }
        public Mock<IDailyBiller>    MoqDailyBiller    { get; } = new Mock<IDailyBiller>();

        public MarketStateDB     MarketState  => MoqMarketState;
        public ChequeVouchersDB  Vouchers     => throw new System.NotImplementedException();
        public ICollectionsDir   Collections  => MoqCollectionsDir.Object;
        public IBalanceDB        Balances     => MoqBalanceDB.Object;
        public IPassbookDB       Passbooks    => MoqPassbookDB.Object;
        public IDailyBiller      DailyBiller  => MoqDailyBiller.Object;

        public bool                 IsValidUser      { get; set; } = true;
        public FirebaseCredentials  Credentials      { get; set; } = new FirebaseCredentials();
        public string               UpdatedCopyPath  { get; set; }
        public SectionDTO           CurrentSection   { get; set; }
        public BankAccountDTO       CurrentBankAcct  { get; set; }

    }
}
