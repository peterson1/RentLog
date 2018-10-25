using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using Newtonsoft.Json;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.RntQueries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RentLog.ImportBYF.Converters
{
    public abstract class DailyTxnConverterBase<T> where T :IDocumentDTO
    {
        protected ITenantDBsDir _rntDir;
        protected RntCache      _rntCache;
        protected ByfCache      _byfCache;


        public DailyTxnConverterBase(MainWindowVM mainWindowVM)
        {
            _rntDir   = mainWindowVM.AppArgs;
            _rntCache = mainWindowVM.RntCache;
            _byfCache = mainWindowVM.ByfCache;
        }


        protected abstract T       CastToDTO          (dynamic byf);
        protected abstract string  DisplayId          { get; }
        protected abstract void    ReplaceInColxnsDB  (IEnumerable<T> rntDTOs, ICollectionsDB colxnsDB);


        public void Rewrite(DateTime date)
        {
            var rntDTOs  = GetCastedsByDate(date);
            var colxnsDb = GetOrCreateColxnsDB(date);
            ReplaceInColxnsDB(rntDTOs, colxnsDb);
        }


        protected IEnumerable<T> GetCastedsByDate(DateTime date)
        {
            var startStr  = CacheReader2.appendToDisplayId(DisplayId);
            var matches   = CacheReader2.findInJsonFiles(_byfCache.CacheDir, startStr);
            var dateMatch = PickOneWithDate(date, matches);
            return dateMatch.Select(_ => (T)CastToDTO(_))
                            .Where (_ => _ != null);
        }


        protected List<dynamic> PickOneWithDate(DateTime date, IEnumerable<string> filePaths)
        {
            foreach (var file in filePaths)
            {
                var lines = File.ReadAllText(file);
                var list  = JsonConvert.DeserializeObject<List<dynamic>>(lines);
                DateTime dateObj = list[0].Date;
                if ((DateTime)list[0].Date == date)
                    return list;
            }
            throw Bad.Data($"No cached json file dated [{date}].");
        }


        protected ICollectionsDB GetOrCreateColxnsDB(DateTime date)
        {
            var dir = _rntDir.Collections;
            return dir.For(date) ?? dir.CreateFor(date);
        }
    }
}
