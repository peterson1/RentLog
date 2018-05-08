using CommonTools.Lib11.DatabaseTools;

namespace RentLog.DomainLib11.Repositories
{
    public abstract class AllReposShimBase<T> : SimpleRepoShimBase<T>
    {
        protected AllRepositories _db;


        public AllReposShimBase(ISimpleRepo<T> simpleRepo, AllRepositories allRepositories) : base(simpleRepo)
        {
            _db = allRepositories;
        }
    }
}
