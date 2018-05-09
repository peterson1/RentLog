using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.ReportRows
{
    public class DailyBillsRow : IHasDTO<DailyBillDTO>
    {
        private Dictionary<int, CollectorDTO> _collectors;

        public DailyBillsRow(LeaseDTO leaseDTO, DailyBillDTO dailyBillDTO, Dictionary<int, CollectorDTO> collectorsDict)
        {
            _collectors = collectorsDict;
            Lease       = leaseDTO;
            DTO         = dailyBillDTO;
            Date        = DTO.GetBillDate();
            Labels      = SetRowLabels();
            Collector   = SetCollector();
        }


        public LeaseDTO      Lease         { get; }
        public DailyBillDTO  DTO           { get; }
        public DateTime      Date          { get; }
        public string        Labels        { get; }
        public CollectorDTO  Collector     { get; }
        public BillState     Rent          => DTO.For(BillCode.Rent    );
        public BillState     Rights        => DTO.For(BillCode.Rights  );
        public BillState     Electric      => DTO.For(BillCode.Electric);
        public BillState     Water         => DTO.For(BillCode.Water   );
        public decimal       TotalPayments => DTO.Bills.Sum(_ => _.TotalPayments);
        public string        Remarks       => DTO.Remarks;
        public int?          PRNumber      => FirstPayment()?.PRNumber;


        private CollectorDTO SetCollector()
        {
            var collector = FirstPayment()?.Collector;
            if (collector == null) return CollectorDTO.Named("--");
            collector.Name = _collectors.TryGetValue(collector.Id, out CollectorDTO match)
                           ? match.Name : $"‹unknown collector [{collector.Id}]›";
            return collector;
        }


        private BillPayment FirstPayment() 
                => Rent    ?.Payments?.FirstOrDefault()
                ?? Rights  ?.Payments?.FirstOrDefault()
                ?? Electric?.Payments?.FirstOrDefault()
                ?? Water   ?.Payments?.FirstOrDefault();


        private string SetRowLabels()
        {
            var labels = new List<string>();

            if (Date < Lease.FirstRentDueDate && Date >= Lease.ContractStart)
                labels.Add("Grace Period");

            if (Date == Lease.ContractStart)
                labels.Add("Contract Start");

            if (Lease is InactiveLeaseDTO lse
                && Date == lse.DeactivatedDate)
                labels.Add(lse.WhyInactive);

            return string.Join(L.f, labels);
        }
    }
}
