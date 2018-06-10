using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class ChequeVouchersDB
    {
        public virtual IFundRequestsRepo    ActiveRequests    { get; set; }
        public virtual IFundRequestsRepo    InactiveRequests  { get; set; }
        public virtual IChequeVouchersRepo  PreparedCheques   { get; set; }


        public void SetAs_Prepared(FundRequestDTO request, 
            DateTime chequeDate, int chequeNumber)
        {
            PreparedCheques.Insert(new ChequeVoucherDTO
            {
                Request      = request,
                ChequeDate   = chequeDate,
                ChequeNumber = chequeNumber,
            });

            InactiveRequests.Insert(request);

            ActiveRequests.Delete(request);
        }


        public void SetAs_Issued(ChequeVoucherDTO dto, DateTime issuedDate, string issuedTo)
        {
            dto.IssuedDate = issuedDate;
            dto.IssuedTo   = issuedTo;
            PreparedCheques.Update(dto);
        }


        public void SetAs_Cleared(ChequeVoucherDTO chq, DateTime date)
        {
            throw new NotImplementedException();
        }


        public void SetAs_TakenBack(ChequeVoucherDTO cheque)
        {
            cheque.IssuedDate = null;
            cheque.IssuedTo   = string.Empty;
            PreparedCheques.Update(cheque);
        }
    }
}
