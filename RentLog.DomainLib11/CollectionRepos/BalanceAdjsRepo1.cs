using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class BalanceAdjsRepo1 : SimpleRepoShimBase<BalanceAdjustmentDTO>, IBalanceAdjustmentsRepo
    {
        private IBalanceDB _balDB;

        public BalanceAdjsRepo1(DateTime date, ISimpleRepo<BalanceAdjustmentDTO> simpleRepo, IBalanceDB balanceDB) : base(simpleRepo)
        {
            _balDB = balanceDB;
            Date   = date;
        }


        public DateTime Date { get; }


        protected override void ExecuteAfterSave(BalanceAdjustmentDTO record) 
            => _balDB.GetRepo(record.LeaseId)
                     .UpdateFrom(Date, record.BillCode);
    }
}
