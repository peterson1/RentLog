using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class CollectorsPerformanceReport : UIList<CollectorPerformanceRow>
    {
        public CollectorsPerformanceReport(ICollectionsDB db, MarketStateDbBase mkt)
        {
            //todo: remove logic from constructor
            List<CollectorDTO> collectors;
            try
            {
                collectors = db.CollectorsSnapshot
                              ?? mkt.Collectors.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (var collector in collectors)
            {
                try
                {
                    this.Add(new CollectorPerformanceRow(collector, db, mkt.Stalls));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            try
            {
                this.RemoveAll(_ => !_.Any());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
