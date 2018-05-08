using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Repositories
{
    public class ActiveLeasesRepo1 : AllReposShimBase<LeaseDTO>, ILeasesRepo
    {
        public ActiveLeasesRepo1(ISimpleRepo<LeaseDTO> simpleRepo, AllRepositories allRepositories) : base(simpleRepo, allRepositories)
        {
        }


        public Dictionary<int, LeaseDTO> StallsLookup()
            => _repo.GetAll().ToDictionary(_ => _.Stall.Id);
    }
}
