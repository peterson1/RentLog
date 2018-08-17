namespace RentLog.DomainLib11.PassbookRepos
{
    public interface IPassbookDB
    {
        IPassbookRowsRepo  GetRepo     (int bankAccountId);
        IPassbookRowsRepo  AllAccounts ();
    }
}
