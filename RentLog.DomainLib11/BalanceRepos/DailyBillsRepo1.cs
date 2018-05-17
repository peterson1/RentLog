using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.BalanceRepos
{
    public class DailyBillsRepo1 : SimpleRepoShimBase<DailyBillDTO>, IDailyBillsRepo
    {
        private LeaseDTO _lse;


        public DailyBillsRepo1(LeaseDTO lease, ISimpleRepo<DailyBillDTO> simpleRepo) : base(simpleRepo)
        {
            _lse = lease;
        }


        public void UpdateFrom(DateTime date, BillCode billCode, IDailyBiller dailyBiller)
        {
            var minID      = date.ToBillID();
            var affctd     = _repo.Find    (_ => _.Id >= minID)
                                  .OrderBy (_ => _.Id).ToList();

            var openingBal = Find(date.AddDays(-1).ToBillID(), false)
                            ?.For(billCode)?.ClosingBalance;

            foreach (var dto in affctd)
            {
                var rowDate  = dto.GetBillDate();
                var newState = dailyBiller.ComputeBill(billCode, _lse, rowDate, openingBal);
                dto.Bills.RemoveAll(_ => _.BillCode == billCode);
                dto.Bills.Add(newState);

                openingBal = newState.ClosingBalance;
            }
            _repo.Update(affctd, true);
        }


        protected override IEnumerable<DailyBillDTO> ToSorted(IEnumerable<DailyBillDTO> items)
            => items.OrderByDescending(_ => _.Id);
    }
}
