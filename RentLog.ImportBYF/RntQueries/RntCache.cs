using CommonTools.Lib11.ExceptionTools;
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


        public LeaseDTO LeaseById(int leaseNid, bool errorIfNoMatch = true)
            => FindById(leaseNid, _lsesById, errorIfNoMatch);


        public BankAccountDTO BankAcctById(int bankAcctNid, bool errorIfNoMatch = true)
            => FindById(bankAcctNid, _bankAcctsById, errorIfNoMatch);


        public CollectorDTO CollectorById(int collectorId, bool errorIfNoMatch = true)
            => FindById(collectorId, _colctrsById, errorIfNoMatch);


        public CollectorDTO CollectorByName(string collectorName, bool errorIfNoMatch = true)
        {
            var matches = _colctrsById.Values.Where(_ => _.Name == collectorName);
            if (matches.Count() == 1) return matches.First();
            if (matches.Count() > 1)
                DuplicateRecordsException.For(matches, "Name", collectorName);

            if (!errorIfNoMatch) return null;
            throw No.Match<CollectorDTO>("Name", collectorName);
        }


        public GLAccountDTO GLAcctById(int gLAccountNid, bool errorIfNoMatch = true)
            => FindById(gLAccountNid, _glAcctsById, errorIfNoMatch);


        private T FindById<T>(int recId, Dictionary<int, T> dict, bool errorIfNoMatch)
        {
            var found = dict.TryGetValue(recId, out T dto);
            if (found) return dto;
            if (!errorIfNoMatch) return default(T);
            throw No.Match<T>("Id", recId);

        }
    }
}
