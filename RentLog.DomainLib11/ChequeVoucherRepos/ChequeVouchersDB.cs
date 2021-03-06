﻿using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class ChequeVouchersDB
    {
        public virtual IFundRequestsRepo    ActiveRequests    { get; set; }
        public virtual IFundRequestsRepo    InactiveRequests  { get; set; }
        public virtual IChequeVouchersRepo  PreparedCheques   { get; set; }
        public virtual IPassbookDB          PassbookRows      { get; set; }

        public ISimpleRepo<FundRequestDTO> AllRequests 
            => new CombinedReadOnlyRepo<FundRequestDTO>(ActiveRequests, InactiveRequests);


        public List<string> GetPayees()
            => ActiveRequests.GetPayees ()
                             .Concat    (InactiveRequests.GetPayees())
                             .GroupBy   (_ => _)
                             .Select    (_ => _.First())
                             .OrderBy   (_ => _)
                             .ToList    ();


        public void SetAs_Prepared(FundRequestDTO request, 
            DateTime chequeDate, int chequeNumber)
        {
            PreparedCheques.Insert(new ChequeVoucherDTO
            {
                Request      = request,
                ChequeDate   = chequeDate,
                ChequeNumber = chequeNumber,
            });
            InactiveRequests.Insert(ToInactive(request));
            ActiveRequests  .Delete(request);
        }


        private FundRequestDTO ToInactive(FundRequestDTO orig)
        {
            var inactv = orig.ShallowClone<FundRequestDTO>();
            inactv.Id  = 0;
            return inactv;
        }


        public int GetNextRequestSerial()
        {
            var activesMax  = ActiveRequests  .GetMaxSerial();
            var inactivsMax = InactiveRequests.GetMaxSerial();
            return Math.Max(activesMax, inactivsMax) + 1;
        }


        public DateTime GetNextRequestDate()
            => ActiveRequests.GetMaxRequestDate();


        public void SetAs_Issued(ChequeVoucherDTO dto, DateTime issuedDate, string issuedTo)
        {
            dto.IssuedDate = issuedDate;
            dto.IssuedTo   = issuedTo;
            PreparedCheques.Update(dto);
        }


        public void SetAs_Cleared(ChequeVoucherDTO chq, DateTime date)
        {
            var passbk = PassbookRows.GetRepo(chq.Request.BankAccountId);
            passbk.InsertClearedCheque(chq, date);
            passbk.RecomputeBalancesFrom(date);
            PreparedCheques.Delete(chq);
        }


        public void SetAs_Cancelled(ChequeVoucherDTO chq)
        {
            var req = FindInactiveRequest(chq);
            req.ChequeStatus = ChequeState.Cancelled;
            InactiveRequests.Update(req);
            PreparedCheques.Delete(chq);
        }


        private FundRequestDTO FindInactiveRequest(ChequeVoucherDTO chq)
        {
            var matches = InactiveRequests.Find(_ => _.SerialNum == chq.Request.SerialNum);

            if (!matches.Any())
            {
                //throw No.Match<FundRequestDTO>("SerialNum", chq.Request.SerialNum);
                InactiveRequests.Insert(chq.Request);
                return FindInactiveRequest(chq);
            }

            if (matches.Count() > 1)
                throw DuplicateRecordsException.For(matches, "SerialNum", chq.Request.SerialNum);

            return matches.Single();
        }


        public void SetAs_Unprepared(ChequeVoucherDTO chq)
        {
            PreparedCheques.Delete(chq);
            var req = FindInactiveRequest(chq);
            InactiveRequests.Delete(req);
            req.Id = 0;
            ActiveRequests.Insert(req);
        }


        public void SetAs_TakenBack(ChequeVoucherDTO cheque)
        {
            cheque.IssuedDate = null;
            cheque.IssuedTo   = string.Empty;
            PreparedCheques.Update(cheque);
        }
    }
}
