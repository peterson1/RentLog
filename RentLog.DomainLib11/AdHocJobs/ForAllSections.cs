using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class ForAllSections
    {
        public static void SetIsActive(bool isActive, ITenantDBsDir dir)
        {
            var repo = dir.MarketState.Sections;
            var secs = repo.GetAll();

            foreach (var sec in secs)
                sec.IsActive = isActive;

            repo.DropAndInsert(secs, true, false);
        }
    }
}
