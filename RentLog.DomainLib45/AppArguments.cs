﻿using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LicenseTools;
using CommonTools.Lib45.LiteDbTools;
using CommonTools.Lib45.ThreadTools;
using Mono.Options;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Repositories;
using RentLog.DomainLib45.Repositories;
using System;

namespace RentLog.DomainLib45
{
    public class AppArguments : ICredentialsProvider, IHasUpdatedCopy
    {
        public AppArguments()
        {
            Parse(Environment.GetCommandLineArgs());

            if (!DbFilePath.IsBlank())
                DB = ConnectToDatabases();
        }


        public string               UpdatedCopyPath  { get; private set; }
        public bool                 IsValidUser      { get; private set; }
        public FirebaseCredentials  Credentials      { get; private set; }

        public string               SystemName       { get; private set; } = "Rent Logs";
        public string               DbFilePath       { get; private set; }

        public AllRepositories      DB               { get; }

        //public IStallsRepo   Stalls         { get; protected set; }
        //public SectionsRepo  Sections       { get; }
        //public ILeasesRepo   ActiveLeases   { get; protected set; }

        public SectionDTO    CurrentSection   { get; set; }


        private AllRepositories ConnectToDatabases()
        {
            var all          = new AllRepositories();
            var db           = new SharedLiteDB(DbFilePath, Credentials?.HumanName ?? "Anonymous");
            all.Stalls       =  new StallsRepo1(new StallsCollection(db), all);
            all.Sections     = new SectionsRepo(db);
            all.ActiveLeases = new ActiveLeasesRepo(db);

            return all;
        }


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
