using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.Authorization;
using System;
using System.Linq;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.MarketStateRepos;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class StallsJob
    {
        public static void RenameOldStalls(ITenantDBsDir dir)
        {
            var repo      = dir.MarketState.Stalls;
            var allStalls = repo.GetAll();

            foreach (var stall in allStalls)
            {
                if (stall.Section.Id <= 3)
                    stall.Name += " (old)";
            }

            repo.DropAndInsert(allStalls, true, false);
        }


        public static Action SetStallDefaults(ITenantDBsDir dir,
            out string jobDesc, out bool canRun)
        {
            canRun  = dir.CanRunAdHocTask(false);
            jobDesc = "Set Stall Defaults";
            return () =>
            {
                var repo = dir.MarketState.Stalls;
                foreach (var stall in repo.GetAll())
                {
                    if (stall.DefaultRent   == null
                     || stall.DefaultRights == null)
                    {
                        var tupl            = FindStallDefaults(stall, dir);
                        stall.DefaultRent   = tupl.Rent;
                        stall.DefaultRights = tupl.Rights;
                        repo.Update(stall);
                    }
                }
            };
        }


        private static (RentParams Rent, RightsParams Rights) FindStallDefaults(StallDTO stall, ITenantDBsDir dir)
        {
            var mkt   = dir.MarketState;
            var match = FindLeaseIn(stall, mkt.ActiveLeases)
                     ?? FindLeaseIn(stall, mkt.InactiveLeases);

            if (match == null)
            {
                stall.Name += " *";//to prevent duplicates
                match = new LeaseDTO
                {
                    Rent   = new RentParams(),
                    Rights = new RightsParams()
                };
            }
            return (match.Rent, match.Rights);
        }


        private static LeaseDTO FindLeaseIn<T>(StallDTO stall, ISimpleRepo<T> repo)
            where T : LeaseDTO
            => repo.GetAll().FindLast(_ => _.Stall.Id == stall.Id);
    }
}
