using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.IntendedColxnConverters
{
    public class IntendedColxnConverter1 : DailyTxnConverterBase<IntendedColxnDTO>
    {
        protected override string DisplayId => "daily_collections_repo?display_id=page_2";

        public IntendedColxnConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        protected override void ReplaceInColxnsDB(IEnumerable<IntendedColxnDTO> rntDTOs, ICollectionsDB colxnsDb)
        {
            var grpBySec = rntDTOs.GroupBy(_ => _.Lease?.Stall?.Section?.Id ?? 0);

            foreach (var secGrp in grpBySec)
            {
                if (secGrp.Key == 0) continue; 
                var repo = colxnsDb.IntendedColxns[secGrp.Key];
                repo.DropAndInsert(secGrp, true, false);
            }
        }


        protected override IntendedColxnDTO CastToDTO(dynamic byf)
        {
            var lseNid   = (int)byf.LeaseNid;
            var rntLease = _rntCache.LeaseById(lseNid);
            var actuals  = new BillAmounts
            {
                Rent     = byf.Rent + byf.Surcharge,
                Rights   = byf.Rights,
                Electric = byf.Electric,
                Water    = byf.Water,
            };
            return new IntendedColxnDTO
            {
                Id            = byf.nid,
                PRNumber      = byf.PrNumber,
                Actuals       = actuals,
                Targets       = actuals,
                Lease         = rntLease,
                StallSnapshot = ((LeaseDTO)rntLease).Stall,
                Remarks       = byf.Remarks,
            };
        }
    }
}
