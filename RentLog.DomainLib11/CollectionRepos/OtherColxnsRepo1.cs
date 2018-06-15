using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class OtherColxnsRepo1 : SimpleRepoShimBase<OtherColxnDTO>, IOtherColxnsRepo
    {
        public OtherColxnsRepo1(ISimpleRepo<OtherColxnDTO> simpleRepo) : base(simpleRepo)
        {
        }


        public override bool IsValidForInsert(OtherColxnDTO draft, out string whyInvalid)
        {
            if (!base.IsValidForInsert(draft, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(draft);
            return whyInvalid.IsBlank();
        }


        public override bool IsValidForUpdate(OtherColxnDTO record, out string whyInvalid)
        {
            if (!base.IsValidForUpdate(record, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(record);
            return whyInvalid.IsBlank();
        }


        private string GetWhyInvalid(OtherColxnDTO dto)
        {
            if (dto.DocumentRef.IsBlank())
                return "PR # should not be blank";

            if (dto.Amount <= 0)
                return "Amount should be greater than zero.";

            if (dto.Collector == null)
                return "“Collector” should not be blank";

            if (dto.GLAccount == null)
                return "“Payment For” should not be blank";

            return string.Empty;
        }
    }
}
