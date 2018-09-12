using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class BalanceAdjsRepo1 : SimpleRepoShimBase<BalanceAdjustmentDTO>, IBalanceAdjustmentsRepo
    {
        private IBalanceDB      _balDB;
        //private ICollectionsDir _colxnsDir;


        public BalanceAdjsRepo1(DateTime date, ISimpleRepo<BalanceAdjustmentDTO> simpleRepo, IBalanceDB balanceDB) : base(simpleRepo)
        {
            _balDB     = balanceDB;
            //_colxnsDir = collectionsDir;
            Date       = date;
        }


        public DateTime Date { get; }


        protected override void ExecuteAfterSave(BalanceAdjustmentDTO record, bool operationIsDelete) 
            => _balDB.GetRepo(record.LeaseId).RecomputeFrom(Date);
    }
}
