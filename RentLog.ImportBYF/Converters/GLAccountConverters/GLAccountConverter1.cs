using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.GLAccountConverters
{
    public class GLAccountConverter1 : ComparisonsListBase
    {
        public GLAccountConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf = byfRecord as dynamic;
            var rem = ((string)byf.Remarks)?.NullIfBlank();
            return new GLAccountDTO
            {
                Id          = byf.nid,
                Name        = byf.Label,
                Remarks     = rem,
                AccountType = GetAcctType((int)byf.AcctTypeTid)
            };
        }


        private GLAcctType GetAcctType(int acctTypeTid)
        {
            switch (acctTypeTid)
            {
                case 33: return GLAcctType.Asset;
                case 34: return GLAcctType.Liability;
                case 35: return GLAcctType.Equity;
                case 36: return GLAcctType.Income;
                case 37: return GLAcctType.Expense;
                default:
                    throw Bad.Data($"Invalid GL Acct type tid: [{acctTypeTid}]");
            }
        }


        public override List<object> GetListFromBYF(string cacheDir)
            => ReadJsonList("gl_accounts_list?display_id=page_2")
                .Select(_ => _ as object).ToList();


        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir dir)
            => dir.MarketState.GLAccounts.GetAll()
                .Select(_ => _ as IDocumentDTO).ToList();


        public override int GetByfId(object byfRecord)
            => (int)((dynamic)byfRecord).nid;


        public override void ReplaceAll(IEnumerable<IDocumentDTO> documents, MarketStateDB mkt)
            => mkt.GLAccounts.DropAndInsert(documents
                .Select(_ => _ as GLAccountDTO), true, false);
    }
}
