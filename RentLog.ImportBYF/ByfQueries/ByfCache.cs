using RentLog.ImportBYF.ByfServerAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public class ByfCache
    {
        private Dictionary<int, string> _terms;


        public string  CacheDir  { get; private set; }
        public bool    IsFilled  { get; private set; }


        public void RefillFromCache(string cacheDir)
        {
            CacheDir = cacheDir;
            var dict = CacheReader2.getTaxonomyTerms(CacheDir);
            _terms   = null;
            _terms   = new Dictionary<int, string>();

            foreach (var kv in dict)
                _terms.Add((int)kv.Key, kv.Value);

            IsFilled = true;
        }


        public async Task RefillFromServer(ByfServerVM byfServer)
        {
            _terms   = await byfServer.Client.GetTaxonomyDictionary();
            IsFilled = true;
        }


        public string Term(int tid) => _terms[tid];
    }
}
