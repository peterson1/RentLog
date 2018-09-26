using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.SectionsRepository
{
    internal class SectionsCollection : NamedCollection<SectionDTO>, ISimpleRepo<SectionDTO>
    {
        internal const string COLXN_NAME = "SectionModel";


        internal SectionsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }
    }
}
