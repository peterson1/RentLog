using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Linq;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class CheckVoucherAdHocs
    {
        public static Action FixBDO1ImportBug(ITenantDBsDir dir, out string jobDesc)
        {
            jobDesc = "Fix BDO-1 Import Bug";
            return () =>
            {
                ProcessRequests(dir.Vouchers.PreparedCheques);
                ProcessRequests(dir.Vouchers.ActiveRequests);
                ProcessRequests(dir.Vouchers.InactiveRequests);
            };
        }


        private static void ProcessRequests(IChequeVouchersRepo repo)
        {
            foreach (var chk in repo.GetAll())
            {
                if (IsBuggy(chk.Request, out AccountAllocation bdo1Alloc))
                {
                    FixBuggyAlloc(bdo1Alloc);
                    if (!repo.Update(chk))
                        throw new Exception("Check Update failed");
                }
            }
        }


        private static void ProcessRequests(ISimpleRepo<FundRequestDTO> repo)
        {
            foreach (var req in repo.GetAll())
            {
                if (IsBuggy(req, out AccountAllocation bdo1Alloc))
                {
                    FixBuggyAlloc(bdo1Alloc);
                    if (!repo.Update(req))
                        throw new Exception("Request Update failed");
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


        private static void FixBuggyAlloc(AccountAllocation bdo1Alloc)
        {
            bdo1Alloc.Account.Id = 0;
            bdo1Alloc.Account.Name = "Cash in Bank: BDO";
        }
    }
}
