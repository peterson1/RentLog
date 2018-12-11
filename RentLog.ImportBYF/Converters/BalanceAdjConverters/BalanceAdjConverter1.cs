using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.BalanceAdjConverters
{
    public class BalanceAdjConverter1 : ConverterRowBase<BalanceAdjustmentDTO>
    {
        public override string Label          => "Balance Adjustments";
        public override string ViewsDisplayID => "balance_adjustments?display_id=page_3";

        private Dictionary<int, DateTime> _adjDates = new Dictionary<int, DateTime>();
        private LeaseDTO _lse;


        public BalanceAdjConverter1(LeaseDTO lease, MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
            _lse = lease;
        }


        public override BalanceAdjustmentDTO CastToRNT(dynamic byf)
        {
            var id        = As.ID(byf.nid);
            var date      = As.Date(byf.date);
            var amount    = GetAmount(byf, out BillCode billCode);
            _adjDates[id] = date;
            return new BalanceAdjustmentDTO
            {
                Id           = id,
                LeaseId      = As.ID(byf.leasenid),
                AmountOffset = amount,
                BillCode     = billCode,
                DocumentRef  = As.Text(byf.referencenum),
                Reason       = As.Text(byf.description),
            };
        }


        private decimal GetAmount(dynamic byf, out BillCode billCode)
        {
            if (TryGetValue(byf.rent     , billCode = BillCode.Rent, out decimal val)) return val;
            if (TryGetValue(byf.surcharge, billCode = BillCode.Rent, out val)) return val;
            if (TryGetValue(byf.rights   , billCode = BillCode.Rights, out val)) return val;
            if (TryGetValue(byf.electric , billCode = BillCode.Electric, out val)) return val;
            if (TryGetValue(byf.water    , billCode = BillCode.Water, out val)) return val;
            throw Bad.Data("Balance Adj. has no valid amount");
        }


        private bool TryGetValue(dynamic dynamic, BillCode billCode, out decimal val)
        {
            var num = (decimal?)As.Decimal_(dynamic);
            val = num ?? 0;
            return num.HasValue;
        }


        public override List<BalanceAdjustmentDTO> GetRntRecords(ITenantDBsDir dir) 
            => throw new NotImplementedException();


        public override Task<List<dynamic>> GetViewsList(string viewsDisplayID)
            => Main.ByfServer.Client.GetViewsList(viewsDisplayID, _lse.Id);


        public override void ReplaceAll(IEnumerable<BalanceAdjustmentDTO> newRecords, MarketStateDbBase mkt)
        {
            foreach (var adj in newRecords)
            {
                var date   = _adjDates[adj.Id];
                var colxns = mkt.Collections.For(date)
                          ?? mkt.Collections.CreateFor(date);
                var adjs   = colxns.BalanceAdjs;
                adjs.Upsert(adj);
            }

            var start = _lse.ContractStart;
            var end   = Main.ByfServer.LastPostedDate;
            var bals  = mkt.Balances.GetRepo(_lse.Id);

            foreach (var day in start.EachDayUpTo(end))
            {
                var recId = day.DaysSinceMin();
                if (!bals.HasId(recId))
                    bals.ProcessBalancedDay(day);
            }
        }
    }
}
