using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;

namespace RentLog.DomainLib11.PassbookRepos
{
    public abstract class VagrantRepoFacadeBase : IPassbookRowsRepo
    {

        public VagrantRepoFacadeBase(int bankAccountId)
        {
            BankAccountID = bankAccountId;
        }


        public int BankAccountID { get; }


        public void InsertClearedCheque(ChequeVoucherDTO cheque, DateTime date)
        {
            var repo = FindRepo(date);
            var row  = cheque.ToPassbookRow();

            if (!repo.IsValidForInsert(row, out string whyNot))
                throw Bad.Insert(row, whyNot);

            repo.Insert(row);
        }


        protected abstract ISimpleRepo<PassbookRowDTO> FindRepo(DateTime date);


        public void RecomputeBalancesFrom(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
