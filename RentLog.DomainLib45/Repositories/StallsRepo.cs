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
            => GetAll().Where(_ => _.Section.Id == section.Id).SortByName();


        protected override SharedCollectionBase<StallDTO> GetSharedCollection(SharedLiteDB sharedLiteDB)
            => new StallsCollection(sharedLiteDB);
    }


    public static class StallsRepoExt
    {
        public static List<StallDTO> SortByName(this IEnumerable<StallDTO> items)
            => items.OrderBy(_ => _.Name).ToList();
    }
}