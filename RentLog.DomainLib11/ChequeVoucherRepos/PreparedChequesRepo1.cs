using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class PreparedChequesRepo1 : SimpleRepoShimBase<ChequeVoucherDTO>, IChequeVouchersRepo
    {
        public PreparedChequesRepo1(ISimpleRepo<ChequeVoucherDTO> simpleRepo) : base(simpleRepo)
        {
        }


        public List<ChequeVoucherDTO> GetIssuedCheques()
            //=> Find(_ => _.IssuedDate.HasValue);
            => GetAll().Where(_ => _.IssuedDate.HasValue).ToList();


        public List<ChequeVoucherDTO> GetNonIssuedCheques()
            //=> Find(_ => !_.IssuedDate.HasValue);
            => GetAll().Where(_ => !_.IssuedDate.HasValue).ToList();


        public override bool IsValidForInsert(ChequeVoucherDTO draft, out string whyInvalid)
        {
            if (!base.IsValidForInsert(draft, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(draft);
            return whyInvalid.IsBlank();
        }


        public override bool IsValidForUpdate(ChequeVoucherDTO record, out string whyInvalid)
        {
            if (!base.IsValidForUpdate(record, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(record);
            return whyInvalid.IsBlank();
        }


        private string GetWhyInvalid(ChequeVoucherDTO dto)
        {
            if (dto.Request == null)
                return "Fund Request should NOT be null.";

            if (!dto.Request.Amount.HasValue)
                return "Requested Amount should have a value.";

            if (dto.ChequeNumber <= 0)
                return "Cheque Number should be greater than zero.";

            if (dto.ChequeDate == DateTime.MinValue)
                return "Cheque Date should not be blank.";

            return string.Empty;
        }
    }
}
