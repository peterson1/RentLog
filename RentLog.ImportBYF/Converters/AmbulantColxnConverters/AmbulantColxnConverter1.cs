using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.AmbulantColxnConverters
{
    public class AmbulantColxnConverter1 : DailyTxnConverterBase<AmbulantColxnDTO>
    {
        protected override string DisplayId => "other_collections_list?display_id=page_2";

        public AmbulantColxnConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        protected override AmbulantColxnDTO CastToDTO(dynamic byf) => new AmbulantColxnDTO
        {
            Id           = byf.nid,
            Amount       = byf.Amount,
            PRNumber     = byf.ReferenceNum,
            ReceivedFrom = byf.ReceivedFrom,
            Remarks      = byf.Remarks
        };


        protected override void ReplaceInColxnsDB(IEnumerable<AmbulantColxnDTO> rntDTOs, ICollectionsDB colxnsDB)
        {
            var grpBySec = rntDTOs.GroupBy(_ => GetSecId(_));
            foreach (var secGrp in grpBySec)
            {
                if (secGrp.Key == 0) continue;
                var repo = colxnsDB.AmbulantColxns[secGrp.Key];
                repo.DropAndInsert(secGrp, true, false);
            }
        }


        private int GetSecId(AmbulantColxnDTO dto)
        {
            if (dto.Remarks.IsBlank()) return 0;
            if (!dto.Remarks.Contains("{")) return 0;
            if (!dto.Remarks.Contains(":")) return 0;
            if (!dto.Remarks.Contains("}")) return 0;
            var secIdStr = dto.Remarks.Between("{", ":");
            return int.TryParse(secIdStr, 
                out int secId) ? secId : 0;
        }
    }
}
