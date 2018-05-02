using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.SectionsRepository;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.Repositories
{
    public class SectionsRepo : SharedCollectionShim<SectionDTO>
    {
        public SectionsRepo(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
        }


        protected override SharedCollectionBase<SectionDTO> GetSharedCollection(SharedLiteDB sharedLiteDB)
            => new SectionsCollection(sharedLiteDB);


        //protected override Func<SectionDTO, object> DefaultSort()
        //    => _ => _.Name;
    }
}
