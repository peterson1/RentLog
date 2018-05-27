using System.Collections.Generic;

namespace RentLog.DomainLib11.Models
{
    public enum BillCode
    {
        Rent     = 1,
        Rights   = 2,
        Electric = 3,
        Water    = 4,
        Other    = 5 
    }


    public class BillCodes
    {
        public static IEnumerable<BillCode> Collected()
            => new List<BillCode> { BillCode.Rent,
                                    BillCode.Rights,
                                    BillCode.Electric,
                                    BillCode.Water };
    }
}
