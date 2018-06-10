using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.PassbookRepos
{
    public class PassbookRowsSimpleRepo : SimpleRepoShimBase<PassbookRowDTO>
    {
        public PassbookRowsSimpleRepo(ISimpleRepo<PassbookRowDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
