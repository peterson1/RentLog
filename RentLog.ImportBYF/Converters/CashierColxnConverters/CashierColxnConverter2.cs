using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.CashierColxnConverters
{
    public class CashierColxnConverter2 : DailyTxnConverterBase2<CashierColxnDTO>
    {
        public CashierColxnConverter2(PeriodRowVM periodRowVM) : base(periodRowVM)
        {
        }


        protected override CashierColxnDTO CastToDTO(dynamic byf) => new CashierColxnDTO
        {
            Id          = byf.nid,
            Amount      = As.DecimalOrZero(byf.rent)
                        + As.DecimalOrZero(byf.rights)
                        + As.DecimalOrZero(byf.electric)
                        + As.DecimalOrZero(byf.water)
                        + As.DecimalOrZero(byf.surcharge),
            DocumentRef = byf.referencenum,
            Remarks     = byf.remarks,
            Lease       = _rntCache.LeaseById(As.ID(byf.leasenid)),
            BillCode    = GetBillCode(byf),
        };


        private BillCode GetBillCode(dynamic byf)
        {
            if (As.DecimalOrZero(byf.rent     ) != 0M) return BillCode.Rent;
            if (As.DecimalOrZero(byf.surcharge) != 0M) return BillCode.Rent;
            if (As.DecimalOrZero(byf.rights   ) != 0M) return BillCode.Rights;
            if (As.DecimalOrZero(byf.electric ) != 0M) return BillCode.Electric;
            if (As.DecimalOrZero(byf.water    ) != 0M) return BillCode.Water;
            return BillCode.Other;
        }


        protected override void ReplaceInColxnsDB(IEnumerable<CashierColxnDTO> rntDTOs, ICollectionsDB colxnsDB)
            => colxnsDB.CashierColxns.DropAndInsert(rntDTOs, true, false);


        protected override Task<List<dynamic>> GetRawBYFsList(ByfClient1 client, DateTime date)
            => client.GetRawByfCashierColxns(date);
    }
}
