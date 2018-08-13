using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.AdHocJobs
{
    public static class AddCollector
    {
        public static void DefaultRecords(int count, ITenantDBsDir dir)
        {
            var recs   = new List<CollectorDTO>();
            var repo   = dir.MarketState.Collectors;
            var lastId = repo.Max(_ => _.Id);
            
            for (int i = 1; i < count + 1; i++)
                recs.Add(CollectorDTO.Named($"Collector #{lastId + i}", true));

            repo.Insert(recs, true);
        }
    }
}
