using CommonTools.Lib11.CollectionTools;
using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.CollectionRepos;
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


        public DailyBillsRepo1(LeaseDTO lease, ISimpleRepo<DailyBillDTO> simpleRepo, ICollectionsDir collectionsDir) : base(simpleRepo)
        {
            _lse   = lease;
            _billr = new DailyBiller1(collectionsDir);
        }


        //public void UpdateFrom(DateTime date)
        //    => BillCodes.Collected().ForEach(_ => UpdateFrom(date, _));


        public void UpdateFrom(DateTime date)
        {
            var dtos = GetRecomputedFrom(date);
            _repo.Update(dtos, true);
        }


        private List<DailyBillDTO> GetRecomputedFrom(DateTime date)
        {
            var minID  = date.ToBillID();
            var affctd = _repo.Find   (_ => _.Id >= minID)
                              .OrderBy(_ => _.Id).ToList();

            foreach (var billCode in BillCodes.Collected())
            {
                var openingBal = Find(date.AddDays(-1).ToBillID(), false)
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


        public void OpenNextDay(DateTime unclosedDate)
        {
            var nextDay = unclosedDate.AddDays(1);
            Upsert(DailyBillDTO.CreateFor(nextDay));

            var dtos = GetRecomputedFrom(unclosedDate);

            foreach (var billCode in BillCodes.Collected())
                dtos.Last().For(billCode).ClosingBalance = null;

            _repo.Update(dtos, true);
        }


        protected override IEnumerable<DailyBillDTO> ToSorted(IEnumerable<DailyBillDTO> items)
            => items.OrderByDescending(_ => _.Id);
    }
}
