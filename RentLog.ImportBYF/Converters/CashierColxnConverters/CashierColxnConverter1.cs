using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.CashierColxnConverters
{
    public class CashierColxnConverter1 : DailyTxnConverterBase<CashierColxnDTO>
    {
        protected override string DisplayId => "balance_adjustments?display_id=page_2";

        public CashierColxnConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        protected override CashierColxnDTO CastToDTO(dynamic byf) => new CashierColxnDTO
        {
            Id          = byf.nid,
            Amount      = byf.Total,
            DocumentRef = byf.ReferenceNum,
            Remarks     = byf.Remarks,
            Lease       = _rntCache.LeaseById((int)byf.LeaseNid),
            BillCode    = GetBillCode(byf),
        };


        private BillCode GetBillCode(dynamic byf)
        {
            if (byf.Rent      != 0) return BillCode.Rent;
            if (byf.Surcharge != 0) return BillCode.Rent;
            if (byf.Rights    != 0) return BillCode.Rights;
            if (byf.Electric  != 0) return BillCode.Electric;
            if (byf.Water     != 0) return BillCode.Water;
            return BillCode.Other;
        }


        protected override void ReplaceInColxnsDB(IEnumerable<CashierColxnDTO> rntDTOs, ICollectionsDB colxnsDB)
            => colxnsDB.CashierColxns.DropAndInsert(rntDTOs, true, false);
    }
}
