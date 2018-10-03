using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.ImportBYF.Converters.SectionConverters
{
    public class SectionConverter1 : ComparisonsListBase
    {
        public SectionConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }

        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf = Cast(byfRecord);
            return new SectionDTO
            {
                Id            = (int)byf.Id.Value,
                Name          = byf.Label.Value,
                StallTemplate = CastStallTemplate(byf)
            };
        }


        private StallDTO CastStallTemplate(ReportModels.Section byf) => new StallDTO
        {
            Name = byf.Label.Value + " {0:000}",
        };


        public override int GetByfId(object byfRecord)
        {
            throw new NotImplementedException();
        }

        public override List<object> GetListFromBYF(string cacheDir)
        {
            throw new NotImplementedException();
        }

        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir appArgs)
        {
            throw new NotImplementedException();
        }


        private ReportModels.Section Cast(object rec)
            => Cast<ReportModels.Section>(rec);


        public override void ReplaceAll(IEnumerable<IDocumentDTO> records, MarketStateDB mkt)
        {
            throw new NotImplementedException();
        }
    }
}
