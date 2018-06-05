using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class StallsRepo1 : MarketStateRepoShimBase<StallDTO>, IStallsRepo
    {
        public StallsRepo1(ISimpleRepo<StallDTO> simpleRepo, MarketStateDB allRepositories) : base(simpleRepo, allRepositories)
        {
        }


        protected override void ValidateBeforeInsert(StallDTO newRecord)
        {
            this.RejectDuplicateRecord(_ => _.Name == newRecord.Name,
                nameof(newRecord.Name), newRecord);
        }


        protected override void ValidateBeforeUpdate(StallDTO changedRecord)
        {
            this.RejectDuplicateRecord(_ => _.Name == changedRecord.Name,
                nameof(changedRecord.Name), changedRecord, _ => _.Id);
        }


        protected override void ValidateBeforeDelete(StallDTO record)
        {
            this.DontDeleteIfOccupied(record, _db.ActiveLeases);
        }


        public List<StallDTO> ForSection(int sectionID)
            => ToSortedList(_repo.GetAll().Where(_ => _.Section.Id == sectionID));


        public List<StallDTO> ForSection(SectionDTO section)
            => ForSection(section.Id);
    }
}
