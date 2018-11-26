using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.Converters.BankAccountConverters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public class ByfCache
    {
        private Dictionary<int, string> _terms;
        private Dictionary<int, string> _payees;


        public string  CacheDir  { get; private set; }
        public bool    IsFilled  { get; private set; }

        public Dictionary<int, int>      BankAcctByGlAcct { get; private set; }
        public Dictionary<int, DateTime> ClearedDatesById { get; } = new Dictionary<int, DateTime>();


        //public void RefillFromCache(string cacheDir)
        //{
        //    CacheDir = cacheDir;
        //    var dict = CacheReader2.getTaxonomyTerms(CacheDir);
        //    _terms   = null;
        //    _terms   = new Dictionary<int, string>();

        //    foreach (var kv in dict)
        //        _terms.Add((int)kv.Key, kv.Value);

        //    IsFilled = true;
        //}


        public async Task RefillFromServer(ByfServerVM byfServer)
        {
            var client       = byfServer.Client;
            _terms           = await client.GetTaxonomyDictionary();
            _payees          = await client.GetPayeesDictionary();
            BankAcctByGlAcct = await client.GetBankAcctsByGLAcctDict();
            IsFilled         = true;
        }


        public string Term      (int tid) => _terms[tid];
        public string PayeeById (int nid) => _payees[nid];
    }
}
