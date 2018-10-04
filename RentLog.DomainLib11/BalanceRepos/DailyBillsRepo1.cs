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


        public void RecomputeFrom(DateTime date)
        {
            var dtos = GetRecomputedFrom(date);
            //_repo.Update(dtos, true);
            _repo.Upsert(dtos, true);
        }


        public void RecomputeAll()
        {
            var dtos = GetRecomputedFrom(_lse.ContractStart);
            _repo.DropAndInsert(dtos, false, true);
        }


        private List<DailyBillDTO> GetRecomputedFrom(DateTime date)
        {
            var minID   = date.DaysSinceMin();
            var matches = _repo.Find(_ => _.Id >= minID);

            //if (matches == null || !matches.Any()) return null;
            //var oldRows = matches.OrderBy(_ => _.Id).ToList();
            //var maxDate = oldRows.Last ().GetBillDate();
            var oldRows = new List<DailyBillDTO>();
            var maxDate = date;
            if (matches?.Any() ?? false)
            {
                oldRows = matches.OrderBy(_ => _.Id).ToList();
                maxDate = oldRows.Last().GetBillDate();
            }

            foreach (var billCode in BillCodes.Collected())
            {
                var openingBal = Find(date.AddDays(-1).DaysSinceMin(), false)
                                    ?.For(billCode)?.ClosingBalance;

                foreach (var rowDate in date.EachDayUpTo(maxDate))
                {
                    var newState = _billr.ComputeBill(billCode, _lse, rowDate, openingBal);

                    var dto = oldRows.SingleOrDefault(_ => _.Id == rowDate.DaysSinceMin());
                    if (dto == null)
                    {
                        dto = DailyBillDTO.CreateFor(rowDate);
                        oldRows.Add(dto);
                    }

                    if (dto.Bills == null) dto.Bills = new List<DailyBillDTO.BillState>();
                    dto.Bills.RemoveAll(_ => _.BillCode == billCode);
                    dto.Bills.Add(newState);

                    openingBal = newState.ClosingBalance;
                }
            }
            return oldRows.OrderBy(_ => _.Id).ToList();
        }


        public void ProcessBalancedDay(DateTime balancedDay)
        {
            var nextDay = balancedDay.AddDays(1);
            Delete(nextDay.DaysSinceMin());
            Insert(DailyBillDTO.CreateFor(nextDay));

            var startId    = _lse.ContractStart.DaysSinceMin();
            var recompDate = _repo.HasId(startId) ? balancedDay : _lse.ContractStart;
            var dtos       = GetRecomputedFrom(recompDate);
            if (dtos == null) return;

            foreach (var billCode in BillCodes.Collected())
                dtos.Last().For(billCode).ClosingBalance = null;

            //_repo.Update(dtos, true);
            _repo.Upsert(dtos, true);
        }


        protected override IEnumerable<DailyBillDTO> ToSorted(IEnumerable<DailyBillDTO> items)
            => items.OrderByDescending(_ => _.Id);
    }
}
