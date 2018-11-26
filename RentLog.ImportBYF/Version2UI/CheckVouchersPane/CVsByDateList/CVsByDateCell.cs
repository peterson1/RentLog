using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList
{
    public class CVsByDateCell
    {
        public List<FundRequestDTO>    ActiveRequests    { get; set; }
        public List<ChequeVoucherDTO>  InactiveRequests  { get; set; }
        public List<ChequeVoucherDTO>  PreparedCheques   { get; set; }

        public bool     IsQueried            => ActiveRequests != null;

        public int?     ActivesHeadCount     => ActiveRequests?.Count;
        public int?     ActivesChildCount    => ActiveRequests?.Sum(_ => _.Allocations.Count);
        public decimal? ActivesTotalAmount   => ActiveRequests?.Sum(_ => _.Amount ?? 0);
        public int?     InactivesHeadCount   => InactiveRequests?.Count;
        public int?     InactivesChildCount  => InactiveRequests?.Sum(_ => _.Request.Allocations.Count);
        public decimal? InactivesTotalAmount => InactiveRequests?.Sum(_ => _.Request.Amount ?? 0);
        public int?     ChecksHeadCount      => PreparedCheques?.Count;
        public int?     ChecksChildCount     => PreparedCheques?.Sum(_ => _.Request.Allocations.Count);
        public decimal? ChecksTotalAmount    => PreparedCheques?.Sum(_ => _.Request.Amount ?? 0);


        public bool Matches(CVsByDateCell othr, out string whyNot)
        {
            if (this.ActivesHeadCount     != othr.ActivesHeadCount    ) return False(whyNot = "ActivesHeadCount mismatch");
            if (this.ActivesChildCount    != othr.ActivesChildCount   ) return False(whyNot = "ActivesChildCount mismatch");
            if (this.ActivesTotalAmount   != othr.ActivesTotalAmount  ) return False(whyNot = "ActivesTotalAmount mismatch");
            if (this.InactivesHeadCount   != othr.InactivesHeadCount  ) return False(whyNot = "InactivesHeadCount mismatch");
            if (this.InactivesChildCount  != othr.InactivesChildCount ) return False(whyNot = "InactivesChildCount mismatch");
            if (this.InactivesTotalAmount != othr.InactivesTotalAmount) return False(whyNot = "InactivesTotalAmount mismatch");
            if (this.ChecksHeadCount      != othr.ChecksHeadCount     ) return False(whyNot = "ChecksHeadCount mismatch");
            if (this.ChecksChildCount     != othr.ChecksChildCount    ) return False(whyNot = "ChecksChildCount mismatch");
            if (this.ChecksTotalAmount    != othr.ChecksTotalAmount   ) return False(whyNot = "ChecksTotalAmount mismatch");
            whyNot = "";
            return true;
        }


        private bool False(string reason) => false;
    }
}
