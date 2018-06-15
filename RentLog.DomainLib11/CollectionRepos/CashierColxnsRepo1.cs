using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class CashierColxnsRepo1 : SimpleRepoShimBase<CashierColxnDTO>, ICashierColxnsRepo
    {
        public CashierColxnsRepo1(ISimpleRepo<CashierColxnDTO> simpleRepo) : base(simpleRepo)
        {
        }


        public override bool IsValidForInsert(CashierColxnDTO draft, out string whyInvalid)
        {
            if (!base.IsValidForInsert(draft, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(draft);
            return whyInvalid.IsBlank();
        }


        public override bool IsValidForUpdate(CashierColxnDTO record, out string whyInvalid)
        {
            if (!base.IsValidForUpdate(record, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(record);
            return whyInvalid.IsBlank();
        }


        private string GetWhyInvalid(CashierColxnDTO dto)
        {
            if (dto.DocumentRef.IsBlank())
                return "PR # should not be blank";

            if (dto.Amount <= 0)
                return "Amount should be greater than zero.";

            if (dto.Lease == null)
                return "“Received From” should be an existing lease";

            if ((int)dto.BillCode < 1)
                return "“Payment For” should not be blank";

            return string.Empty;
        }
    }
}
