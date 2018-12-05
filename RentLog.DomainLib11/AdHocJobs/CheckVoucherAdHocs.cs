using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Linq;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class CheckVoucherAdHocs
    {
        //public static Action FixBDO1ImportBug(ITenantDBsDir dir, out string jobDesc)
        //{
        //    jobDesc = "Fix BDO-1 Import Bug";
        //    return () =>
        //    {
        //        //ProcessRequests(dir.Vouchers.PreparedCheques);
        //        ProcessRequests(dir.Vouchers.ActiveRequests);
        //        ProcessRequests(dir.Vouchers.InactiveRequests);
        //    };
        //}
        public static void FixBDO1ImportBug(ITenantDBsDir dir)
        {
            //ProcessRequests(dir.Vouchers.PreparedCheques);
            ProcessRequests(dir.Vouchers.ActiveRequests);
            ProcessRequests(dir.Vouchers.InactiveRequests);
        }


        private static void ProcessRequests(ISimpleRepo<FundRequestDTO> repo)
        {
            foreach (var req in repo.GetAll())
            {
                //if (req.SerialNum == 4603)
                //{
                //    var a = "dsf";
                //}

                if (IsBuggy(req, out AccountAllocation bdo1Alloc))
                {
                    req.Allocations.RemoveAll(_ => _.Account.Id == 0);
                    //req.Allocations.RemoveAll(_ => _.Account.Id == 37282);
                    bdo1Alloc.Account.Id   = 0;
                    bdo1Alloc.Account.Name = "Cash in Bank: BDO";
                    if (!repo.Update(req))
                        throw new Exception("Update failed");
                }
            }
        }


        private static bool IsBuggy(FundRequestDTO req, out AccountAllocation bdo1Alloc)
        {
            bdo1Alloc = null;
            //if (req.IsBalanced) return false;
            if (req.Allocations == null) return false;
            if (!req.Allocations.Any()) return false;

            //Cash in Bank - BDO 1
            var matches = req.Allocations.Where(_ 
                        => _.Account.Id == 37282);

            if (!matches.Any()) return false;
            if (matches.Count() > 1) return false;

            bdo1Alloc = matches.Single();
            return true; 
        }
    }
}
