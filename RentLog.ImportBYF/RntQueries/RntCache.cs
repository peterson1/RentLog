using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.RntQueries
{
    public class RntCache
    {
        private Dictionary<int, LeaseDTO> _lsesById;

        public void RefillFrom(ITenantDBsDir dir)
        {
            var mkt   = dir.MarketState;
            _lsesById = mkt.GetAllLeases().ToDictionary(_ => _.Id);
        }


        internal LeaseDTO LeaseById(int leaseNid)
            => _lsesById[leaseNid];
    }
}
