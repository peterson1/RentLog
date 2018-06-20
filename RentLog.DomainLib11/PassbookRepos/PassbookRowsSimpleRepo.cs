using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.PassbookRepos
{
    public class PassbookRowsSimpleRepo : SimpleRepoShimBase<PassbookRowDTO>
    {
        public PassbookRowsSimpleRepo(ISimpleRepo<PassbookRowDTO> simpleRepo) : base(simpleRepo)
        {
        }


        protected override void ValidateBeforeInsert(PassbookRowDTO newRecord)
        {
            if (!IsValidForInsert(newRecord, out string whyNot))
                throw Bad.Insert(newRecord, whyNot);
        }
    }
}
