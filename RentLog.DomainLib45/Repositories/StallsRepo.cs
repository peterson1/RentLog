using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.Repositories
{
    //todo: make this Lib11
    public class StallsRepo : SharedCollectionShim<StallDTO>, IStallsRepo
    {
        public StallsRepo(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
        }

        protected override SharedCollectionBase<StallDTO> GetSharedCollection(SharedLiteDB sharedLiteDB)
            => new StallsCollection(sharedLiteDB);


        public override IEnumerable<StallDTO> ToSorted(IEnumerable<StallDTO> items)
            => items.OrderBy(_ => _.Name);


        public List<StallDTO> ForSection(SectionDTO section)
            => ToSortedList(GetAll().Where(_ => _.Section.Id == section?.Id));


        protected override void BeforeInsert(StallDTO newRecord)
        {
            var matches = GetAll().Where(_ => _.Name == newRecord.Name);

            if (matches.Any())
                throw DuplicateRecordsException.For(matches, nameof(newRecord.Name), newRecord.Name);
        }
    }
}