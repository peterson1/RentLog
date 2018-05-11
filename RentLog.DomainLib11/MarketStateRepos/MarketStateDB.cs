namespace RentLog.DomainLib11.MarketStateRepos
{
    public class MarketStateDB
    {
        public virtual IStallsRepo      Stalls        { get; set; }
        public virtual ISectionsRepo    Sections      { get; set; }
        public virtual ICollectorsRepo  Collectors    { get; set; }
        public virtual ILeasesRepo      ActiveLeases  { get; set; }
        public virtual string           BranchName    { get; set; }
    }
}
