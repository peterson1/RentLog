using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BalanceRepos
{
    public class DailyBillsRepo1 : SimpleRepoShimBase<DailyBillDTO>, IDailyBillsRepo
    {
        public DailyBillsRepo1(ISimpleRepo<DailyBillDTO> simpleRepo) : base(simpleRepo)
        {
        }


        public void UpdateFrom(DateTime date, BillCode billCode, ICollectionsDB colxnsDB)
        {
            var minID      = date.ToBillID();
            var affctd     = _repo.Find    (_ => _.Id >= minID)
                                  .OrderBy (_ => _.Id).ToList();

            var openingBal = Find(date.AddDays(-1).ToBillID(), false)
                            ?.For(billCode)?.ClosingBalance;

            foreach (var dto in affctd)
            {
                //var colxnsDB =  subAppArgs.GetColxnsDB(dto.IdToBusinessDate());
                //var newState = ZRead.GetBillState(billCode, lease, colxnsDB, openingBal);
                //dto.SetBillState(newState);
                var newState = RecomputeBill(dto, billCode, colxnsDB);
                dto.Bills.RemoveAll(_ => _.BillCode == billCode);
                dto.Bills.Add(newState);

                openingBal = newState.ClosingBalance;
            }
            _repo.Update(affctd, true);
        }


        private BillState RecomputeBill(DailyBillDTO bill, BillCode billCode, ICollectionsDB colxnsDB)
        {
            throw new NotImplementedException();
        }


        protected override IOrderedEnumerable<DailyBillDTO> ToSorted(IEnumerable<DailyBillDTO> items)
            => items.OrderByDescending(_ => _.Id);
    }
}
