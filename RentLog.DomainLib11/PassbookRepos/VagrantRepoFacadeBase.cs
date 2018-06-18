using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.DomainLib11.Validations;
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

        protected abstract ISimpleRepo<PassbookRowDTO> FindRepo(DateTime date);


        public void InsertClearedCheque(ChequeVoucherDTO cheque, DateTime clearedDate)
        {
            var whyNot = cheque.WhyInvalidForSetAsCleared(BankAccountID);
            if (!whyNot.IsBlank())
                throw Bad.Insert(cheque, whyNot);

            var repo = FindRepo(clearedDate);
            var row  = cheque.ToPassbookRow(clearedDate);
            if (!repo.IsValidForInsert(row, out whyNot))
                throw Bad.Insert(row, whyNot);

            repo.Insert(row);
        }


        public void InsertDepositedColxn(BankAccountDTO bankDeposit)
        {
            throw new NotImplementedException();
        }


        public void RecomputeBalancesFrom(DateTime date)
        {
            //todo: implement this
            //throw new NotImplementedException();
        }
    }
}
