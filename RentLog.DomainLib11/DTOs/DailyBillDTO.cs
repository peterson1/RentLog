using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ReflectionTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.DTOs
{
    public enum BillCode
    {
        Rent     = 1,
        Rights   = 2,
        Electric = 3,
        Water    = 4,
        Other    = 5 
    }


    public class DailyBillDTO : IDocumentDTO, ICloneable
    {
        public int        Id         { get; set; }
        public string     Author     { get; set; }
        public DateTime   Timestamp  { get; set; }
        public string     Remarks    { get; set; }


        public List<BillState> Bills { get; set; }


        public class BillState
        {
            public BillCode   BillCode        { get; set; }
            public decimal?   OpeningBalance  { get; set; }
            public decimal?   ClosingBalance  { get; set; }

            public List<BillPayment>      Payments     { get; set; }
            public List<BillPenalty>      Penalties    { get; set; }
            public List<BillAdjustment>   Adjustments  { get; set; }

            public decimal TotalPayments    => Payments   ?.Sum(_ => _.Amount      ) ?? 0;
            public decimal TotalPenalties   => Penalties  ?.Sum(_ => _.Amount      ) ?? 0;
            public decimal TotalAdjustments => Adjustments?.Sum(_ => _.AmountOffset) ?? 0;
        }


        public class BillPayment
        {
            public CollectorDTO   Collector   { get; set; }
            public int            PRNumber    { get; set; }
            public decimal        Amount      { get; set; }
            public string         Remarks     { get; set; }
        }

        public class BillPenalty
        {
            public string   Label        { get; set; }
            public decimal  Amount       { get; set; }
            public string   Computation  { get; set; }
        }

        public class BillAdjustment
        {
            public string   AdjustedBy    { get; set; }
            public decimal  AmountOffset  { get; set; }
            public string   Reason        { get; set; }
            public string   DocumentRef   { get; set; }
        }


        public BillState For(BillCode billCode)
            => Bills?.SingleOrDefault(_ => _.BillCode == billCode);

        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }


    public static class DailyBillDTOExtensions
    {
        public static DateTime GetBillDate(this DailyBillDTO dto)
            => DateTime.MinValue.AddDays(dto.Id);


        public static int ToBillID(this DateTime businessDate)
            => (businessDate - DateTime.MinValue).Days;
    }
}
