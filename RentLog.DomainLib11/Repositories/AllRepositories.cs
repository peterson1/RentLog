namespace RentLog.DomainLib11.Repositories
{
    public class AllRepositories
    {
        public virtual IStallsRepo    Stalls        { get; set; }
        public virtual ISectionsRepo  Sections      { get; set; }
        public virtual ILeasesRepo    ActiveLeases  { get; set; }
    }
}
