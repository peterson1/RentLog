using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.DTOs
{
    public class BalanceAdjustmentDTO : DocumentDTOBase
    {
        public BillCode    BillCode      { get; set; }
        public int         LeaseId       { get; set; }
        public decimal     AmountOffset  { get; set; }
        public string      Reason        { get; set; }
        public string      DocumentRef   { get; set; }

        public override bool Equals(object obj)
        {
            var dTO = obj as BalanceAdjustmentDTO;
            return dTO != null &&
                   BillCode == dTO.BillCode &&
                   LeaseId == dTO.LeaseId &&
                   AmountOffset == dTO.AmountOffset &&
                   Reason == dTO.Reason &&
                   DocumentRef == dTO.DocumentRef;
        }

        public override int GetHashCode()
        {
            var hashCode = 1537752341;
            hashCode = hashCode * -1521134295 + BillCode.GetHashCode();
            hashCode = hashCode * -1521134295 + LeaseId.GetHashCode();
            hashCode = hashCode * -1521134295 + AmountOffset.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Reason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DocumentRef);
            return hashCode;
        }

        internal BillAdjustment ToBillAdjustment() => new BillAdjustment
        {
            DocumentRef  = this.DocumentRef,
            AdjustedBy   = this.Author,
            AmountOffset = this.AmountOffset,
            Reason       = this.Reason,
        };

        public static bool operator ==(BalanceAdjustmentDTO dTO1, BalanceAdjustmentDTO dTO2)
        {
            return EqualityComparer<BalanceAdjustmentDTO>.Default.Equals(dTO1, dTO2);
        }

        public static bool operator !=(BalanceAdjustmentDTO dTO1, BalanceAdjustmentDTO dTO2)
        {
            return !(dTO1 == dTO2);
        }


        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(null, obj)) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    if (obj.GetType() != this.GetType()) return false;

        //    var othr = obj as BalanceAdjustmentDTO;
        //    return (othr != null)
        //        && (this.BillCode     == othr.BillCode    )
        //        && (this.LeaseId      == othr.LeaseId     )
        //        && (this.AmountOffset == othr.AmountOffset)
        //        && (this.Reason       == othr.Reason      )
        //        && (this.DocumentRef  == othr.DocumentRef );
        //}
    }
}
