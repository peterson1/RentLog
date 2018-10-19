using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.RntQueries
{
    public class RntCache
    {
        private Dictionary<int, LeaseDTO>       _lsesById;
        private Dictionary<int, BankAccountDTO> _bankAcctsById;
        private Dictionary<int, CollectorDTO>   _colctrsById;
        private Dictionary<int, GLAccountDTO>   _glAcctsById;


        public void RefillFrom(ITenantDBsDir dir)
        {
            var mkt        = dir.MarketState;
            _lsesById      = mkt.GetAllLeases().ToDictionary(_ => _.Id);
            _bankAcctsById = mkt.BankAccounts.GetAll().ToDictionary(_ => _.Id);
            _colctrsById   = mkt.Collectors.GetAll().ToDictionary(_ => _.Id);
            _glAcctsById   = mkt.GLAccounts.GetAll().ToDictionary(_ => _.Id);
        }


        public LeaseDTO LeaseById(int leaseNid)
            => _lsesById.TryGetValue(leaseNid, out LeaseDTO lse)
                ? lse : new LeaseDTO();


        public BankAccountDTO BankAcctById(int bankAcctNid)
            => _bankAcctsById[bankAcctNid];


        public CollectorDTO CollectorById(int collectorId)
            => _colctrsById[collectorId];


        public GLAccountDTO GLAcctById(int gLAccountNid)
            => _glAcctsById[gLAccountNid];
    }
}
