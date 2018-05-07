using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.SectionsRepository;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.Repositories
{
    public class SectionsRepo : SharedCollectionShim<SectionDTO>, ISectionsRepo
    {
        public SectionsRepo(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
        }


        protected override SharedCollectionBase<SectionDTO> GetSharedCollection(SharedLiteDB sharedLiteDB)
            => new SectionsCollection(sharedLiteDB);


        public override List<SectionDTO> GetAll() => ToSortedList(_colxn.GetAll());


        public override IEnumerable<SectionDTO> ToSorted(IEnumerable<SectionDTO> items)
            => items.OrderBy(_ => _.Name);
    }
}
