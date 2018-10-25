using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.RntQueries;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters
{
    public abstract class DailyTxnConverterBase2<T> where T : IDocumentDTO
    {
        protected ITenantDBsDir _rntDir;
        protected RntCache      _rntCache;
        protected ByfCache      _byfCache;
        protected ByfClient1    _client;
        protected PeriodRowVM   _row;


        public DailyTxnConverterBase2(PeriodRowVM periodRowVM)
        {
            _row      = periodRowVM;
            _rntDir   = _row.MainWindow.AppArgs;
            _rntCache = _row.MainWindow.RntCache;
            _byfCache = _row.MainWindow.ByfCache;
            _client   = _row.MainWindow.ByfServer.Client;
        }


        protected abstract T                    CastToDTO          (dynamic byf);
        protected abstract Task<List<dynamic>>  GetRawBYFsList     (ByfClient1 client, DateTime date);
        protected abstract void                 ReplaceInColxnsDB  (IEnumerable<T> rntDTOs, ICollectionsDB colxnsDB);


        public async Task Rewrite(DateTime date)
        {
            var rntDTOs  = await GetCastedsByDate(date);
            var colxnsDb = GetOrCreateColxnsDB(date);
            ReplaceInColxnsDB(rntDTOs, colxnsDb);
        }


        protected async Task<IEnumerable<T>> GetCastedsByDate(DateTime date)
        {
            var byfList = await GetRawBYFsList(_client, date);
            var rntList = new List<T>();

            foreach (var byf in byfList)
                rntList.Add(CastToDTO(byf));

            return rntList;
        }


        private ICollectionsDB GetOrCreateColxnsDB(DateTime date)
        {
            var dir = _rntDir.Collections;
            return dir.For(date) ?? dir.CreateFor(date);
        }
    }
}
