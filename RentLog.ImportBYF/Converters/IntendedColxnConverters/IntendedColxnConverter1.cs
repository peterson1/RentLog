using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Linq;

namespace RentLog.ImportBYF.Converters.IntendedColxnConverters
{
    public class IntendedColxnConverter1 : DailyTxnConverterBase<IntendedColxnDTO>
    {
        protected override string DisplayId => "daily_collections_repo?display_id=page_2";

        public IntendedColxnConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        public override void Rewrite(DateTime date)
        {
            var rntDTOs   = GetCastedsByDate(date);
            var grpBySec  = rntDTOs.GroupBy(_ => _.Lease.Stall.Section.Id);
            var colxnsDir = _rntDir.Collections;
            var colxnsDb  = colxnsDir.For(date) ?? colxnsDir.CreateFor(date);

            foreach (var secGrp in grpBySec)
            {
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
