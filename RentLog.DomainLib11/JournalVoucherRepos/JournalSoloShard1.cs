using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.JournalVoucherRepos
{
    public class JournalSoloShard1 : SimpleRepoShimBase<JournalVoucherDTO>
    {
        public JournalSoloShard1(ISimpleRepo<JournalVoucherDTO> simpleRepo) : base(simpleRepo)
        {
        }


        public override bool IsValidForInsert(JournalVoucherDTO draft, out string whyInvalid)
        {
            if (!base.IsValidForInsert(draft, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(draft);
            return whyInvalid.IsBlank();
        }


        public override bool IsValidForUpdate(JournalVoucherDTO record, out string whyInvalid)
        {
            if (!base.IsValidForUpdate(record, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(record);
            return whyInvalid.IsBlank();
        }


        private string GetWhyInvalid(JournalVoucherDTO dto)
        {
            if (dto.SerialNum <= 0)
                return "Voucher Number should be greater than zero.";

            if (dto.Description.IsBlank())
                return "Description should NOT be blank.";

            if (dto.TransactionDate == DateTime.MinValue)
                return "Transaction-Date should NOT be blank.";

            if (dto.Amount <= 0)
                return "Amount should be greater than zero.";

            if (!dto.IsBalanced)
                return $"Debits should match Credits (diff: {(dto.TotalCredit - dto.TotalDebit):N2})";

            return string.Empty;
        }
    }
}
