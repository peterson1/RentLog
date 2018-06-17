using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface IUncollectedsRepo : ISimpleRepo<UncollectedLeaseDTO>
    {
        List<UncollectedLeaseDTO> InferUncollecteds
            (IEnumerable<IntendedColxnDTO>    intendedColxns, 
             IEnumerable<UncollectedLeaseDTO> didNotOperate);

        decimal? GetDue(LeaseDTO lse, BillCode billCode);
    }
}
