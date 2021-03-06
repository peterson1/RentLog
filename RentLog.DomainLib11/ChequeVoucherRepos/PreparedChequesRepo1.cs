﻿using CommonTools.Lib11.DatabaseTools;
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


        public List<ChequeVoucherDTO> GetIssuedCheques(int bankAcctID)
            => GetAll().Where(_ => _.Request.BankAccountId == bankAcctID 
                               &&  _.IssuedDate.HasValue).ToList();


        public List<ChequeVoucherDTO> GetNonIssuedCheques(int bankAcctID)
            => GetAll().Where(_ => _.Request.BankAccountId == bankAcctID
                               && !_.IssuedDate.HasValue).ToList();


        protected override string GetWhyInvalid(ChequeVoucherDTO dto)
        {
            if (dto.Request == null)
                return "Fund Request should NOT be null.";

            if (!dto.Request.Amount.HasValue)
                return "Requested Amount should have a value.";

            if (dto.Request.BankAccountId <= 0)
                return "Bank Account ID should be greater than zero.";

            if (dto.ChequeNumber <= 0)
                return "Cheque Number should be greater than zero.";

            if (dto.ChequeDate == DateTime.MinValue)
                return "Cheque Date should not be blank.";

            return string.Empty;
        }
    }
}
