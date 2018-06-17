using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.LicenseTools;
using CommonTools.Lib45.ThreadTools;
using Mono.Options;
using RentLog.DatabaseLib.DatabaseFinders;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.PassbookRepos;
using System;

namespace RentLog.DomainLib45
{
    public class AppArguments : ITenantDBsDir
    {
        public AppArguments()
        {
            Parse(Environment.GetCommandLineArgs());
            CurrentUser = Credentials?.HumanName ?? "Anonymous";
            MarketState = GetMarketStateDB();
            Vouchers    = GetChequeVouchersDB        (MarketState);
            Balances    = new BalancesLocalDir       (MarketState);
            Collections = new CollectionsLocalDir    (MarketState);
            Passbooks   = new TransactionsByMonthDir (MarketState);
        }


        public string               UpdatedCopyPath  { get; private set; }
        public bool                 IsValidUser      { get; protected set; }
        public FirebaseCredentials  Credentials      { get; protected set; }
        public string               CurrentUser      { get; }

        public string               DbFilePath       { get; private set; }
        public string               Param1           { get; private set; }

        public MarketStateDB        MarketState      { get; }
        public ChequeVouchersDB     Vouchers         { get; }
        public ICollectionsDir      Collections      { get; }
        public IBalanceDB           Balances         { get; }
        public IDailyBiller         DailyBiller      { get; } //todo: use this instance for all
        public IPassbookDB          Passbooks        { get; }
        public SectionDTO           CurrentSection   { get; set; }
        public BankAccountDTO       CurrentBankAcct  { get; set; }


        protected virtual MarketStateDB GetMarketStateDB() 
            => new MarketStateDBFile(DbFilePath, CurrentUser);


        protected ChequeVouchersDB GetChequeVouchersDB(MarketStateDB marketState)
            => new PassbookDBFile(marketState);


        private void SetCredentials(string key)
        {
            IsValidUser = SeatLicenser.TryGetCredentials(key, 
                out FirebaseCredentials creds, out string err);

            Credentials = creds;

#if DEBUG
            Credentials.Roles = "Cashier";
#endif
        }


        private void Parse(string[] commandLineArgs)
        {
            var options = new OptionSet
            {
                { "db|database=" , "Database file path", db  => DbFilePath  = db   },
                {"exe|origexe="  , "Original exe path" , exe => UpdatedCopyPath = exe  },
                {"key|publickey=", "Public key"        , key => SetCredentials(key)},
                { "p1|param1="   , "Parameter 1"       , p1  => Param1 = p1 },
            };
            try
            {
                options.Parse(commandLineArgs);
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
            }
        }
    }
}
