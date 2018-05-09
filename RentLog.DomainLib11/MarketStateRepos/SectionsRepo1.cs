﻿using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class SectionsRepo1 : MarketStateRepoShimBase<SectionDTO>, ISectionsRepo
    {
        public SectionsRepo1(ISimpleRepo<SectionDTO> simpleRepo, MarketStateDB allRepositories) : base(simpleRepo, allRepositories)
        {
        }


        protected override void ValidateBeforeInsert(SectionDTO newRecord)
        {
            this.RejectDuplicateRecord(_ => _.Name == newRecord.Name, 
                nameof(newRecord.Name), newRecord);
        }


        protected override void ValidateBeforeUpdate(SectionDTO changedRecord)
        {
            this.RejectDuplicateRecord(_ => _.Name == changedRecord.Name,
                nameof(changedRecord.Name), changedRecord);
        }
    }
}
