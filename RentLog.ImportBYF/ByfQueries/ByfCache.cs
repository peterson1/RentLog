using System;
using System.Collections.Generic;

namespace RentLog.ImportBYF.ByfQueries
{
    public class ByfCache
    {
        private Dictionary<int, string> _terms;


        public string   Dir   { get; private set; }


        public void RefillFrom(string cacheDir)
        {
            Dir      = cacheDir;
            var dict = CacheReader2.getTaxonomyTerms(Dir);
            _terms   = null;
            _terms   = new Dictionary<int, string>();

            foreach (var kv in dict)
                _terms.Add((int)kv.Key, kv.Value);
        }


        public string Term(int tid) => _terms[tid];
    }
}
