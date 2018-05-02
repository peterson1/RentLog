using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.SectionsRepository
{
    public class SectionsCollection : NamedCollectionBase<SectionDTO>
    {
        private const string COLXN_NAME = "SectionModel";


        public SectionsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
