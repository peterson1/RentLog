using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class FixBdoImportBug
    {
        public static Action Run(ITenantDBsDir dir, out string jobDesc)
        {
            jobDesc   = "Fix BDO-MM DCDR Import Bug";
            return () =>
            {
                //ProcessRequests(dir.Vouchers.PreparedCheques);
                //ProcessRequests(dir.Vouchers.ActiveRequests);
                //ProcessRequests(dir.Vouchers.InactiveRequests);
            };
        }
    }
}
