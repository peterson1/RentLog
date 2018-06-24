using CommonTools.Lib11.FileSystemTools;
using CommonTools.Lib11.GoogleTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.JournalVoucherRepos;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.PassbookRepos;

namespace RentLog.DomainLib11.DataSources
{
    public interface ITenantDBsDir : ICredentialsProvider, IHasUpdatedCopy
    {
        MarketStateDB         MarketState   { get; }
        ChequeVouchersDB      Vouchers      { get; }
        ICollectionsDir       Collections   { get; }
        IBalanceDB            Balances      { get; }
        IPassbookDB           Passbooks     { get; }
        IDailyBiller          DailyBiller   { get; }
        IJournalVouchersRepo  Journals      { get; }

        SectionDTO        CurrentSection   { get; set; }
        BankAccountDTO    CurrentBankAcct  { get; set; }
    }
}
