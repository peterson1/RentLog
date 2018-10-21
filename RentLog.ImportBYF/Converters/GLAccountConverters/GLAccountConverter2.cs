using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.GLAccountConverters
{
    public class GLAccountConverter2 : ConverterRowBase<GLAccountDTO>
    {
        public override string Label          => "GL Accounts";
        public override string ViewsDisplayID => "gl_accounts_list?display_id=page_2";

        public GLAccountConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override GLAccountDTO CastToRNT(dynamic byf)
        {
            var typ = GetAcctType(AsID(byf.accttypetid));
            return new GLAccountDTO
            {
                Id          = AsID(byf.nid),
                Name        = AsText(byf.label),
                Remarks     = AsText(byf.remarks),
                AccountType = typ
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


        public override List<GLAccountDTO> GetRntRecords(ITenantDBsDir dir)
            => dir.MarketState.GLAccounts.GetAll();


        public override void ReplaceAll(IEnumerable<GLAccountDTO> newRecords, MarketStateDB mkt) 
            => mkt.GLAccounts.DropAndInsert(newRecords, true, false);
    }
}
