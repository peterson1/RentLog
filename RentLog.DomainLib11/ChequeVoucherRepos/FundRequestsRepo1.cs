using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.VoucherRules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class FundRequestsRepo1 : SimpleRepoShimBase<FundRequestDTO>, IFundRequestsRepo
    {
        public FundRequestsRepo1(ISimpleRepo<FundRequestDTO> simpleRepo) : base(simpleRepo)
        {
        }

        public DateTime GetMaxRequestDate() 
            => !Any() ? DateTime.Now : Max(_ => _.RequestDate);


        public int GetMaxSerial() 
            => !Any() ? 0 : Max(_ => _.SerialNum);


        public List<string> GetPayees()
            => GetAll().Select  (_ => _.Payee)
                       .GroupBy (_ => _)
                       .Select  (_ => _.First())
                       .ToList  ();


        public override bool IsValidForInsert(FundRequestDTO draft, out string whyInvalid)
        {
            if (!base.IsValidForInsert(draft, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(draft);
            return whyInvalid.IsBlank();
        }


        public override bool IsValidForUpdate(FundRequestDTO record, out string whyInvalid)
        {
            if (!base.IsValidForUpdate(record, out whyInvalid)) return false;
            whyInvalid = GetWhyInvalid(record);
            return whyInvalid.IsBlank();
        }


        private string GetWhyInvalid(FundRequestDTO dto)
        {
            if (dto.SerialNum <= 0)
                return "Voucher Number should be greater than zero.";

            if (dto.BankAccountId <= 0)
                return "Bank Acct ID should be greater than zero.";

            if (dto.Payee.IsBlank())
                return "Payee should NOT be blank.";

            if (dto.Purpose.IsBlank())
                return "Purpose should NOT be blank.";

            if (dto.RequestDate == DateTime.MinValue)
                return "Request-Date should NOT be blank.";

            if ((dto.Amount ?? 0) <= 0)
                return "Amount should be greater than zero.";

            if (!dto.IsBalanced)
                return $"Debits should match Credits (diff: {(dto.TotalCredit - dto.TotalDebit):N2})";

            if (!dto.HasCashInBankEntry())
                return "Voucher should have at least 1 “Cash in Bank” entry.";

            return string.Empty;
        }
    }
}
