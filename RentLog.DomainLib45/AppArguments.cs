using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LicenseTools;
using CommonTools.Lib45.LiteDbTools;
using CommonTools.Lib45.ThreadTools;
using Mono.Options;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.Repositories;
using System;

namespace RentLog.DomainLib45
{
    public class AppArguments : ICredentialsProvider, IHasUpdatedCopy
    {
        public AppArguments()
        {
            Parse(Environment.GetCommandLineArgs());
            var db   = new SharedLiteDB(DbFilePath, Credentials?.HumanName ?? "Anonymous");
            Stalls   = new StallsRepo(db);
            Sections = new SectionsRepo(db);
        }


        public string               UpdatedCopyPath  { get; private set; }
        public bool                 IsValidUser      { get; private set; }
        public FirebaseCredentials  Credentials      { get; private set; }

        public string               SystemName       { get; private set; } = "Rent Logs";
        public string               DbFilePath       { get; private set; }

        public StallsRepo           Stalls           { get; }
        public SectionsRepo         Sections         { get; }


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
