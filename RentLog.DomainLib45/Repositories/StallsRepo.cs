using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.Repositories
{
    public class StallsRepo : SharedCollectionShim<StallDTO>
    {
        public StallsRepo(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
        }


        public List<StallDTO> ForSection(SectionDTO section)
            => ToSortedList(GetAll().Where(_ => _.Section.Id == section?.Id));


        protected override SharedCollectionBase<StallDTO> GetSharedCollection(SharedLiteDB sharedLiteDB)
            => new StallsCollection(sharedLiteDB);


        protected override IEnumerable<StallDTO> ToSorted(IEnumerable<StallDTO> items)
            => items.OrderBy(_ => _.Name);
    }
}