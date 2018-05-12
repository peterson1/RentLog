using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class BalanceAdjsRepo1 : SimpleRepoShimBase<BalanceAdjustmentDTO>, IBalanceAdjustmentsRepo
    {
        private IBalanceDB     _balDB;
        private ICollectionsDB _colxnsDB;

        public BalanceAdjsRepo1(DateTime date, ISimpleRepo<BalanceAdjustmentDTO> simpleRepo, IBalanceDB balanceDB, ICollectionsDB collectionsDB) : base(simpleRepo)
        {
            _balDB    = balanceDB;
            _colxnsDB = collectionsDB;
            Date      = date;
        }


        public DateTime Date { get; }


        protected override void ExecuteAfterSave(BalanceAdjustmentDTO record) 
            => _balDB.GetRepo(record.LeaseId)
                     .UpdateFrom(Date, record.BillCode, _colxnsDB);
    }
}
