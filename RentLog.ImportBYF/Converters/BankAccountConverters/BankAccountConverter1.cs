using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.BankAccountConverters
{
    public class BankAccountConverter1 : ComparisonsListBase
    {
        public BankAccountConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf = Cast(byfRecord);
            return new BankAccountDTO
            {
                Id                 = (int)byf.Id.Value,
                Name               = byf.Label.Value,
                MaintainingBalance = 50000,
            };
        }


        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir dir)
            => dir.MarketState.BankAccounts.GetAll()
                .Select(_ => _ as IDocumentDTO).ToList();


        public override List<object> GetListFromBYF(string cacheDir)
            => CacheReader2.getBankAccounts(cacheDir).Values
                .Select(_ => _ as object).ToList();


        public override int GetByfId(object byfRecord)
            => (int)Cast(byfRecord).Id.Value;


        private ReportModels.BankAccount Cast(object rec)
            => Cast<ReportModels.BankAccount>(rec);


        public override void ReplaceAll(IEnumerable<IDocumentDTO> documents, MarketStateDB mkt)
            => mkt.BankAccounts.DropAndInsert(documents
                .Select(_ => _ as BankAccountDTO), true, false);
    }
}
