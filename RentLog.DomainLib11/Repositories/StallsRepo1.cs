using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Repositories
{
    public class StallsRepo1 : AllReposShimBase<StallDTO>, IStallsRepo
    {
        public StallsRepo1(ISimpleRepo<StallDTO> simpleRepo, AllRepositories allRepositories) : base(simpleRepo, allRepositories)
        {
        }


        protected override void ValidateBeforeInsert(StallDTO newRecord)
        {
            this.RejectDuplicateName(newRecord);
        }


        protected override void ValidateBeforeDelete(StallDTO record)
        {
            this.DontDeleteIfOccupied(record, _db.ActiveLeases);
        }


        public List<StallDTO> ForSection(SectionDTO section)
            => ToSortedList(_repo.GetAll().Where(_ => _.Section.Id == section.Id));
    }
}
