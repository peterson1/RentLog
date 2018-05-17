using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class BalanceAdjsRepo1 : SimpleRepoShimBase<BalanceAdjustmentDTO>, IBalanceAdjustmentsRepo
    {
        private IBalanceDB      _balDB;
        private ICollectionsDir _colxnsDir;
        private IDailyBiller    _billr;


        public BalanceAdjsRepo1(DateTime date, ISimpleRepo<BalanceAdjustmentDTO> simpleRepo, IBalanceDB balanceDB, ICollectionsDir collectionsDir) : base(simpleRepo)
        {
            _balDB     = balanceDB;
            _colxnsDir = collectionsDir;
            Date       = date;
            _billr     = new DailyBiller1(collectionsDir);
        }


        public DateTime Date { get; }


        protected override void ExecuteAfterSave(BalanceAdjustmentDTO record) 
            => _balDB.GetRepo(record.LeaseId)
                     .UpdateFrom(Date, record.BillCode, _billr);
    }
}
