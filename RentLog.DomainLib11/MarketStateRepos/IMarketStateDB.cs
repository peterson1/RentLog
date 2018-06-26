using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public interface IMarketStateDB
    {
        IActiveLeasesRepo ActiveLeases { get; set; }
        IBalanceDB Balances { get; set; }
        IBankAccountsRepo BankAccounts { get; set; }
        string BranchName { get; set; }
        ICollectionsDir Collections { get; set; }
        ICollectorsRepo Collectors { get; set; }
        string CurrentUser { get; set; }
        string DatabasePath { get; set; }
        IGLAccountsRepo GLAccounts { get; set; }
        IInactiveLeasesRepo InactiveLeases { get; set; }
        ISectionsRepo Sections { get; set; }
        IStallsRepo Stalls { get; set; }
        string SystemName { get; set; }

        LeaseDTO FindLease(int leaseID);
        void RefreshStall(LeaseDTO lease);
    }
}