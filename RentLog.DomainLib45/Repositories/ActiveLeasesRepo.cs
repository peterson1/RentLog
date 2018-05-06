using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.LeasesRepository;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.Repositories
{
    public class ActiveLeasesRepo : SharedCollectionShim<LeaseDTO>, ILeasesRepo
    {
        public ActiveLeasesRepo(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
        }


        protected override SharedCollectionBase<LeaseDTO> GetSharedCollection(SharedLiteDB sharedLiteDB)
            => new ActiveLeasesCollection(sharedLiteDB);


        public override IEnumerable<LeaseDTO> ToSorted(IEnumerable<LeaseDTO> items)
            => items.OrderByDescending(_ => _.Id);


        public Dictionary<int, LeaseDTO> StallsLookup()
            => GetAll().ToDictionary(_ => _.Stall.Id);
    }
}