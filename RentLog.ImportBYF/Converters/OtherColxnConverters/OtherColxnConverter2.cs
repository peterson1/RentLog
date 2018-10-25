using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.OtherColxnConverters
{
    public class OtherColxnConverter2 : DailyTxnConverterBase2<OtherColxnDTO>
    {
        public OtherColxnConverter2(PeriodRowVM periodRowVM) : base(periodRowVM)
        {
        }


        protected override OtherColxnDTO CastToDTO(dynamic byf)
        {
            var byfColctr = As.Text(byf.receivedfrom);
            var rntColctr = _rntCache.CollectorByName(byfColctr, false);
            var rntGLAcct = _rntCache.GLAcctById(As.ID(byf.glaccountnid));
            var payFor    = _byfCache.Term(As.ID(byf.paymentfortid));
            return new OtherColxnDTO
            {
                Id          = byf.nid,
                Amount      = byf.amount,
                DocumentRef = byf.referencenum,
                Collector   = rntColctr,
                GLAccount   = rntGLAcct,
                Remarks     = $"from: {byfColctr}"
                            + $"{L.f}{payFor}"
                            + $"{L.f}{byf.remarks}",
            };
        }


        protected override void ReplaceInColxnsDB(IEnumerable<OtherColxnDTO> rntDTOs, ICollectionsDB colxnsDB)
            => colxnsDB.OtherColxns.DropAndInsert(rntDTOs, true, false);


        protected override Task<List<dynamic>> GetRawBYFsList(ByfClient1 client, DateTime date)
            => client.GetRawByfOtherColxns(date);
    }
}
