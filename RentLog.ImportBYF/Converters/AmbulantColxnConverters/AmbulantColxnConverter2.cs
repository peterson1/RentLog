using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.AmbulantColxnConverters
{
    public class AmbulantColxnConverter2 : DailyTxnConverterBase2<AmbulantColxnDTO>
    {
        public AmbulantColxnConverter2(PeriodRowVM periodRowVM) : base(periodRowVM)
        {
        }


        protected override AmbulantColxnDTO CastToDTO(dynamic byf) => new AmbulantColxnDTO
        {
            Id           = As.ID(byf.nid),
            Amount       = As.Decimal(byf.amount),
            PRNumber     = As.ID(byf.referencenum),
            ReceivedFrom = As.Text(byf.receivedfrom),
            Remarks      = As.Text(byf.remarks)
        };


        private int GetSecId(AmbulantColxnDTO dto)
        {
            var secIdStr = dto.Remarks.Between("{", ":");
            return int.TryParse(secIdStr,
                out int secId) ? secId : 0;
        }


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


        protected override Task<List<dynamic>> GetRawBYFsList(ByfClient1 client, DateTime date)
            => client.GetRawByfAmbulantColxns(date);
    }
}
