using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var byfRem   = (string)byf.Remarks;
            var colctrId = int.Parse(byfRem.Between(":", "}", true));
            var payFor   = _byfCache.Term((int)byf.PaymentForTid);
            var rntRem   = $"from: {byf.ReceivedFrom}"
                         + $"{L.f}{payFor}"
                         + $"{L.f}{byf.Remarks}";

            return new OtherColxnDTO
            {
                Id          = byf.nid,
                Amount      = byf.Amount,
                DocumentRef = byf.ReferenceNum,
                Remarks     = rntRem,
                Collector   = _rntCache.CollectorById(colctrId),
                GLAccount   = _rntCache.GLAcctById((int)byf.GLAccountNid)
            };
        }

        protected override void ReplaceInColxnsDB(IEnumerable<OtherColxnDTO> rntDTOs, ICollectionsDB colxnsDB)
            => colxnsDB.OtherColxns.DropAndInsert(rntDTOs, true, false);
    }
}
