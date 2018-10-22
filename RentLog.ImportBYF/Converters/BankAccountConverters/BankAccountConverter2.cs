using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.BankAccountConverters
{
    public class BankAccountConverter2 : ConverterRowBase<BankAccountDTO>
    {
        public override string Label          => "Bank Accounts";
        public override string ViewsDisplayID => "banks_list?display_id=page_2";

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


        public override void ReplaceAll(IEnumerable<BankAccountDTO> newRecords, MarketStateDB mkt)
            => mkt.BankAccounts.DropAndInsert(newRecords, true, false);
    }
}
