using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface INoOperationsRepo : ISimpleRepo<UncollectedLeaseDTO>
    {
    }
}
