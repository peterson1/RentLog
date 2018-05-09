using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class DailyBillsRow : IHasDTO<DailyBillDTO>
    {
        public DailyBillsRow(DailyBillDTO dailyBillDTO)
        {
            DTO = dailyBillDTO;
        }


        public DailyBillDTO            DTO           { get; }
        public DateTime                Date          => DTO.GetBillDate();
        public DailyBillDTO.BillState  Rent          => DTO.For(BillCode.Rent    );
        public DailyBillDTO.BillState  Rights        => DTO.For(BillCode.Rights  );
        public DailyBillDTO.BillState  Electric      => DTO.For(BillCode.Electric);
        public DailyBillDTO.BillState  Water         => DTO.For(BillCode.Water   );
        public decimal                 TotalPayments => DTO.Bills.Sum(_ => _.TotalPayments);
        public string                  Remarks       => DTO.Remarks;

    }
}
