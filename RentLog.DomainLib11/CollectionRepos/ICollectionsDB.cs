namespace RentLog.DomainLib11.CollectionRepos
{
    public interface ICollectionsDB
    {
        bool IsPosted();

        ICashierColxnsRepo  CashierColxns  { get; }
    }
}
