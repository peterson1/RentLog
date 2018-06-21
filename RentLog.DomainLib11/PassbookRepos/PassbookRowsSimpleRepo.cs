using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

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


        protected override IEnumerable<PassbookRowDTO> ToSorted(IEnumerable<PassbookRowDTO> items)
            => items.OrderBy (_ => _.DateOffset)
                    .ThenBy  (_ => _.Id);


        public override bool IsValidForInsert(PassbookRowDTO draft, out string whyInvalid)
        {
            if (!base.IsValidForInsert(draft, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(draft);
            return whyInvalid.IsBlank();
        }


        public override bool IsValidForUpdate(PassbookRowDTO record, out string whyInvalid)
        {
            if (!base.IsValidForUpdate(record, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(record);
            return whyInvalid.IsBlank();
        }


        private string GetWhyInvalid(PassbookRowDTO dto)
        {
            if (dto.Amount == 0)
                return "Amount should not be zero.";

            if (dto.TransactionRef.IsBlank())
                return "“Transaction ref #” should not be blank";

            if (dto.Subject.IsBlank())
                return "Subject should not be blank";

            if (dto.Description.IsBlank())
                return "Description should not be blank";

            if (dto.DateOffset <= 0)
                return "DateOffset should be greater than zero.";

            if (dto.DocRefType.IsBlank())
                return "DocRefType should not be blank";

            if (dto.DocRefId < 0)
                return "DocRefId should at least be zero.";

            if (dto.DocRefJson.IsBlank())
                return "DocRefJson should not be blank";

            return string.Empty;
        }
    }
}
