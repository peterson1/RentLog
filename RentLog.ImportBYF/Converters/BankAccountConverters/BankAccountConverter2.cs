using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.BankAccountConverters
{
    public class BankAccountConverter2 : ConverterRowBase<BankAccountDTO>
    {
        public const    string VIEWS_ID       =  "banks_list?display_id=page_2";
        public override string Label          => "Bank Accounts";
        public override string ViewsDisplayID => VIEWS_ID;

        public BankAccountConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override BankAccountDTO CastToRNT(dynamic byf) => new BankAccountDTO
        {
            Id                 = As.ID(byf.nid),
            Name               = As.Text(byf.bankname),
            MaintainingBalance = 50_000
        };


        public override List<BankAccountDTO> GetRntRecords(ITenantDBsDir dir)
            => dir.MarketState.BankAccounts.GetAll();


        public override void ReplaceAll(IEnumerable<BankAccountDTO> newRecords, MarketStateDbBase mkt)
            => mkt.BankAccounts.DropAndInsert(newRecords, true, false);
    }


    public static class BankAcctClientExtensions
    {
        public static async Task<Dictionary<int, int>> GetBankAcctsByGLAcctDict(this ByfClient1 client)
        {
            var dict     = new Dictionary<int, int>();
            var dynamics = await client.GetViewsList(BankAccountConverter2.VIEWS_ID);

            foreach (var byf in dynamics)
            {
                var glAcctId = As.ID(byf.glaccountnid);
                var bankAcct = As.ID(byf.nid);
                dict.Add(glAcctId, bankAcct);
            }
            return dict;
        }
    }
}
