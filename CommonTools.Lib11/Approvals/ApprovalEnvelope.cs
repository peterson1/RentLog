using System;

namespace CommonTools.Lib11.Approvals
{
    public class ApprovalEnvelope<T>
    {
        public string     RequestKey       { get; set; }
        public T          Content          { get; set; }
        public DateTime   RequestDate      { get; set; }
        public string     RequesterName    { get; set; }
        public string     RequestRemarks   { get; set; }

        public DateTime?  ResponseDate     { get; set; }
        public string     ResponderName    { get; set; }
        public bool?      IsApproved       { get; set; }
        public string     ResponseRemarks  { get; set; }


    }
}
