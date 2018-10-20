using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public class LeaseConverter2 : LeaseConverter1
    {
        public LeaseConverter2(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf = byfRecord as dynamic;

            throw new NotImplementedException();
        }


        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir appArgs)
        {
            var mkt = appArgs.MarketState;
            var actives = mkt.ActiveLeases.GetAll()
                             .Select(_ => _ as IDocumentDTO);
            var inactvs = mkt.InactiveLeases.GetAll()
                             .Select(_ => _ as IDocumentDTO);
            return actives.Concat(inactvs).ToList();
        }


        public override List<object> GetListFromBYF(string cacheDir)
            => ReadJsonList("leases_list?display_id=page_2")
                .Select(_ => _ as object).ToList();


        public override int GetByfId(object byfRecord)
            => (int)((dynamic)byfRecord).nid;
    }
}
