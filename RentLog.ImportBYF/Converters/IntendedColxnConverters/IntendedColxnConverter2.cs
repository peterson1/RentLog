using CommonTools.Lib11.DynamicTools;
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

namespace RentLog.ImportBYF.Converters.IntendedColxnConverters
{
    public class IntendedColxnConverter2 : DailyTxnConverterBase2<IntendedColxnDTO>
    {
        public IntendedColxnConverter2(PeriodRowVM periodRowVM) : base(periodRowVM)
        {
        }


        protected override IntendedColxnDTO CastToDTO(dynamic byf)
        {
            var lseNid   = As.ID(byf.leasenid);
            var rntLease = _rntCache.LeaseById(lseNid, false);
            var actuals  = new BillAmounts
            {
                Rent     = As.Decimal(byf.rent) + As.Decimal(byf.surcharge),
                Rights   = As.Decimal(byf.rights),
                Electric = As.Decimal(byf.electric),
                Water    = As.Decimal(byf.water),
            };
            return new IntendedColxnDTO
            {
                Id            = As.ID(byf.nid),
                PRNumber      = As.ID(byf.prnumber),
                Actuals       = actuals,
                Targets       = actuals,
                Lease         = rntLease,
                StallSnapshot = rntLease?.Stall,
                Remarks       = As.Text(byf.remarks),
            };
        }


        protected override void ReplaceInColxnsDB(IEnumerable<IntendedColxnDTO> rntDTOs, ICollectionsDB colxnsDB)
        {
            var grpBySec = rntDTOs.GroupBy(_ => _.Lease?.Stall?.Section?.Id ?? 0);
            foreach (var secGrp in grpBySec)
            {
                if (secGrp.Key == 0) continue;
                var repo = colxnsDB.IntendedColxns[secGrp.Key];
                repo.DropAndInsert(secGrp, true, false);
            }
        }


        protected override Task<List<dynamic>> GetRawBYFsList(ByfClient1 client, DateTime date)
            => client.GetRawByfLeaseColxns(date);
    }
}
