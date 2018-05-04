using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.LeasesRepository;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.Repositories
{
    public class ActiveLeasesRepo : SharedCollectionShim<LeaseDTO>
    {
        public ActiveLeasesRepo(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
        }


        protected override SharedCollectionBase<LeaseDTO> GetSharedCollection(SharedLiteDB sharedLiteDB)
            => new ActiveLeasesCollection(sharedLiteDB);


        protected override IEnumerable<LeaseDTO> ToSorted(IEnumerable<LeaseDTO> items)
            => items.OrderByDescending(_ => _.Id);
    }
}
