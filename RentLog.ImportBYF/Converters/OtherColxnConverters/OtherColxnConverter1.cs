using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.OtherColxnConverters
{
    public class OtherColxnConverter1 : DailyTxnConverterBase<OtherColxnDTO>
    {
        protected override string DisplayId => "other_collections_list?display_id=page_2";

        public OtherColxnConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        protected override OtherColxnDTO CastToDTO(dynamic byf)
        {
            if (byf.PaymentForTid != 32) return null;
            var byfColctr = (string)byf.ReceivedFrom;
            var rntColctr = _rntCache.CollectorByName(byfColctr, false);
            var rntGLAcct = _rntCache.GLAcctById((int)byf.GLAccountNid);
            var payFor    = _byfCache.Term((int)byf.PaymentForTid);

            return new OtherColxnDTO
            {
                Id          = byf.nid,
                Amount      = byf.Amount,
                DocumentRef = byf.ReferenceNum,
                Collector   = rntColctr,
                GLAccount   = rntGLAcct,
                Remarks     = $"from: {byfColctr}"
                            + $"{L.f}{payFor}"
                            + $"{L.f}{byf.Remarks}",
            };
        }


        protected override void ReplaceInColxnsDB(IEnumerable<OtherColxnDTO> rntDTOs, ICollectionsDB colxnsDB)
            => colxnsDB.OtherColxns.DropAndInsert(rntDTOs, true, false);
    }
}
