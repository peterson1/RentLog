﻿using System;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class ChequeVouchersDB
    {
        public virtual IFundRequestsRepo    ActiveRequests    { get; set; }
        public virtual IFundRequestsRepo    InactiveRequests  { get; set; }
        public virtual IChequeVouchersRepo  PreparedCheques   { get; set; }
        //public virtual IChequeVouchersRepo  IssuedCheques     { get; set; }


        public void SetAs_Prepared(FundRequestDTO request, 
            DateTime chequeDate, int chequeNumber)
        {
            throw new NotImplementedException();
        }
    }
}
