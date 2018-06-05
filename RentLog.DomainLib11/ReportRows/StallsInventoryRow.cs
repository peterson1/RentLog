using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.MathTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class StallsInventoryRow
    {
        public StallsInventoryRow(SectionDTO section, ICollectionsDB collectionsDB)
        {
            Section = section;
            Occupieds.SetItems(GetOccupieds(collectionsDB));
            Vacants.SetItems(GetVacants(collectionsDB));
        }


        public SectionDTO        Section    { get; }
        public UIList<LeaseDTO>  Occupieds  { get; } = new UIList<LeaseDTO>();
        public UIList<StallDTO>  Vacants    { get; } = new UIList<StallDTO>();


        public int      TotalCount    => Occupieds.Count + Vacants.Count;
        public decimal  TotalRent     => OccupiedRent + VacantRent;
        public decimal  OccupiedRate  => Occupieds.Count.PercentOf(TotalCount);
        public decimal  VacantRate    => Vacants  .Count.PercentOf(TotalCount);
        public decimal  OccupiedRent  => Occupieds.Sum(_ => _.Rent.RegularRate);
        public decimal  VacantRent    => Vacants  .Sum(_ => _.DefaultRent.RegularRate);


        private IEnumerable<StallDTO> GetVacants(ICollectionsDB db)
            => db.VacantStalls[Section.Id].GetAll();


        private IEnumerable<LeaseDTO> GetOccupieds(ICollectionsDB db)
        {
            var lses = db.IntendedColxns[Section.Id]
                         .GetAll().Select(_ => _.Lease).ToList();

            lses.AddRange(db.Uncollecteds[Section.Id]
                        .GetAll().Select(_ => _.Lease));

            lses.AddRange(db.NoOperations[Section.Id]
                        .GetAll().Select(_ => _.Lease));
            return lses;
        }
    }
}
