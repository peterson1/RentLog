using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LicenseTools;
using CommonTools.Lib45.ThreadTools;
using Mono.Options;
using RentLog.DatabaseLib.DatabaseFinders;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;

namespace RentLog.DomainLib45
{
    public class AppArguments : ICredentialsProvider, IHasUpdatedCopy
    {
        public AppArguments()
        {
            Parse(Environment.GetCommandLineArgs());
            CurrentUser = Credentials?.HumanName ?? "Anonymous";
            MarketState = MarketStateDBFile.Load(DbFilePath, CurrentUser);
            Collections = new CollectionsDBFolder(DbFilePath, CurrentUser);
            Balances    = new BalancesDBFolder(DbFilePath, CurrentUser);
        }


        public string               UpdatedCopyPath  { get; private set; }
        public bool                 IsValidUser      { get; private set; }
        public FirebaseCredentials  Credentials      { get; private set; }
        public string               CurrentUser      { get; }

        public string               SystemName       { get; private set; } = "Rent Logs";
        public string               DbFilePath       { get; private set; }

        public MarketStateDB        MarketState      { get; }
        public ICollectionDBs       Collections      { get; }
        public IBalanceDBs          Balances         { get; }
        public SectionDTO           CurrentSection   { get; set; }


        private void SetCredentials(string key)
        {
            IsValidUser = SeatLicenser.TryGetCredentials(key, 
                out FirebaseCredentials creds, out string err);

            Credentials = creds;
        }


        private void Parse(string[] commandLineArgs)
        {
            var options = new OptionSet
            {
                { "db|database=" , "Database file path", db  => DbFilePath  = db   },
                {"exe|origexe="  , "Original exe path" , exe => UpdatedCopyPath = exe  },
                {"key|publickey=", "Public key"        , key => SetCredentials(key)}
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
