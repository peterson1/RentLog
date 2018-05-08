using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Repositories
{
    public class SectionsRepo1 : AllReposShimBase<SectionDTO>, ISectionsRepo
    {
        public SectionsRepo1(ISimpleRepo<SectionDTO> simpleRepo, AllRepositories allRepositories) : base(simpleRepo, allRepositories)
        {
        }
    }
}
