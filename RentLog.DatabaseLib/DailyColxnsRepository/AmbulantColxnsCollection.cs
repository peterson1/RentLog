﻿using CommonTools.Lib45.LiteDbTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.DailyColxnsRepository
{
    internal class AmbulantColxnsCollection : NamedCollection<AmbulantColxnDTO>
    {
        private const string COLXN_NAME_FMT = "Section{0}_FromAmbulants";


        internal AmbulantColxnsCollection(SectionDTO section, SharedLiteDB sharedLiteDB) 
            : base(GetCollectionName(section), sharedLiteDB)
        {
        }


        private static string GetCollectionName(SectionDTO section)
            => string.Format(COLXN_NAME_FMT, section.Id);
    }
}
