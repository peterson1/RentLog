using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.DailyColxnsRepository
{
    internal class NoOperationsCollection : NamedCollection<UncollectedLeaseDTO>
    {
        private const string COLXN_NAME_FMT = "Section{0}_NoOperations2";

        internal NoOperationsCollection(SectionDTO section, SharedLiteDB sharedLiteDB)
            : base(GetCollectionName(section), sharedLiteDB)
        {
        }


        private static string GetCollectionName(SectionDTO section)
            => string.Format(COLXN_NAME_FMT, section.Id);
    }
}
