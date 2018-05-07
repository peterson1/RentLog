namespace RentLog.DomainLib11.Repositories
{
    public class AllRepositories
    {
        public IStallsRepo    Stalls        { get; set; }
        public ISectionsRepo  Sections      { get; set; }
        public ILeasesRepo    ActiveLeases  { get; set; }
    }
}
