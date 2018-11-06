using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList
{
    public class CVsByDateCell
    {
        public List<FundRequestDTO>    ActiveRequests    { get; set; }
        public List<FundRequestDTO>    InactiveRequests  { get; set; }
        public List<ChequeVoucherDTO>  RequestedChecks   { get; set; }
        
        public bool     IsQueried            => ActiveRequests != null;

        public int?     ActivesHeadCount     => ActiveRequests?.Count;
        public int?     ActivesChildCount    => ActiveRequests?.Sum(_ => _.Allocations.Count);
        public decimal? ActivesTotalAmount   => ActiveRequests?.Sum(_ => _.Amount ?? 0);
        public int?     InactivesHeadCount   => InactiveRequests?.Count;
        public int?     InactivesChildCount  => InactiveRequests?.Sum(_ => _.Allocations.Count);
        public decimal? InactivesTotalAmount => InactiveRequests?.Sum(_ => _.Amount ?? 0);
        public int?     ChecksHeadCount      => RequestedChecks?.Count;
        public int?     ChecksChildCount     => RequestedChecks?.Sum(_ => _.Request.Allocations.Count);
        public decimal? ChecksTotalAmount    => RequestedChecks?.Sum(_ => _.Request.Amount ?? 0);


        public bool Matches(CVsByDateCell byfCell, out string whyNot)
        {
            throw new NotImplementedException();
        }
    }
}
