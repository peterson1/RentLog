using CommonTools.Lib11.CollectionTools;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.BalanceRepos
{
    public class DailyBillsRepo1 : SimpleRepoShimBase<DailyBillDTO>, IDailyBillsRepo
    {
        private LeaseDTO     _lse;
        private IDailyBiller _billr;


        public DailyBillsRepo1(LeaseDTO lease, ISimpleRepo<DailyBillDTO> simpleRepo, ITenantDBsDir tenantDBsDir) : base(simpleRepo)
        {
            _lse   = lease;
            _billr = tenantDBsDir.DailyBiller;
        }


        public void UpdateFrom(DateTime date)
        {
            var dtos = GetRecomputedFrom(date);
            //_repo.Update(dtos, true);
            _repo.Upsert(dtos, true);
        }


        private List<DailyBillDTO> GetRecomputedFrom(DateTime date)
        {
            var minID   = date.DaysSinceMin();
            var oldRows = _repo.Find(_ => _.Id >= minID)
                               .OrderBy(_ => _.Id).ToList();
            var minDate = oldRows.First().GetBillDate();
            var maxDate = oldRows.Last ().GetBillDate();
            var newRows = new List<DailyBillDTO>();

            foreach (var billCode in BillCodes.Collected())
            {
                var openingBal = Find(date.AddDays(-1).DaysSinceMin(), false)
                                    ?.For(billCode)?.ClosingBalance;

                foreach (var rowDate in minDate.EachDayUpTo(maxDate))
                {
                    var newState = _billr.ComputeBill(billCode, _lse, rowDate, openingBal);

                    var dto = oldRows.SingleOrDefault(_ => _.Id == rowDate.DaysSinceMin())
                           ?? DailyBillDTO.CreateFor(rowDate);

                    if (dto.Bills == null) dto.Bills = new List<DailyBillDTO.BillState>();
                    dto.Bills.RemoveAll(_ => _.BillCode == billCode);
                    dto.Bills.Add(newState);

                    openingBal = newState.ClosingBalance;
                    newRows.Add(dto);
                }
            }
            return newRows;
        }


        private List<DailyBillDTO> GetRecomputedFrom_deprecate(DateTime date)
        {
            var minID  = date.DaysSinceMin();
            var affctd = _repo.Find   (_ => _.Id >= minID)
                              .OrderBy(_ => _.Id).ToList();

            foreach (var billCode in BillCodes.Collected())
            {
                var openingBal = Find(date.AddDays(-1).DaysSinceMin(), false)
                                    ?.For(billCode)?.ClosingBalance;

                foreach (var dto in affctd)
                {
                    var rowDate  = dto.GetBillDate();
                    var newState = _billr.ComputeBill(billCode, _lse, rowDate, openingBal);
                    if (dto.Bills == null) dto.Bills = new List<DailyBillDTO.BillState>();
                    dto.Bills.RemoveAll(_ => _.BillCode == billCode);
                    dto.Bills.Add(newState);

                    openingBal = newState.ClosingBalance;
                }
            }
            return affctd;
        }


        public void ProcessBalancedDay(DateTime balancedDay)
        {
            var nextDay = balancedDay.AddDays(1);
            Delete(nextDay.DaysSinceMin());
            Insert(DailyBillDTO.CreateFor(nextDay));

            var dtos = GetRecomputedFrom(balancedDay);

            foreach (var billCode in BillCodes.Collected())
                dtos.Last().For(billCode).ClosingBalance = null;

            _repo.Update(dtos, true);
        }


        protected override IEnumerable<DailyBillDTO> ToSorted(IEnumerable<DailyBillDTO> items)
            => items.OrderByDescending(_ => _.Id);
    }
}
