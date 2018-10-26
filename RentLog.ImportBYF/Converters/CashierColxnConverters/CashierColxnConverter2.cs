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
            Amount      = As.Decimal(byf.rent)
                        + As.Decimal(byf.rights)
                        + As.Decimal(byf.electric)
                        + As.Decimal(byf.water)
                        + As.Decimal(byf.surcharge),
            DocumentRef = byf.referencenum,
            Remarks     = byf.remarks,
            Lease       = _rntCache.LeaseById(As.ID(byf.leasenid)),
            BillCode    = GetBillCode(byf),
        };


        private BillCode GetBillCode(dynamic byf)
        {
            if (byf.rent      != 0) return BillCode.Rent;
            if (byf.surcharge != 0) return BillCode.Rent;
            if (byf.rights    != 0) return BillCode.Rights;
            if (byf.electric  != 0) return BillCode.Electric;
            if (byf.water     != 0) return BillCode.Water;
            return BillCode.Other;
        }


        protected override void ReplaceInColxnsDB(IEnumerable<CashierColxnDTO> rntDTOs, ICollectionsDB colxnsDB)
            => colxnsDB.CashierColxns.DropAndInsert(rntDTOs, true, false);


        protected override Task<List<dynamic>> GetRawBYFsList(ByfClient1 client, DateTime date)
            => client.GetRawByfCashierColxns(date);
    }
}
