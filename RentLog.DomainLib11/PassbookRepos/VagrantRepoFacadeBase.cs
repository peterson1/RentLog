using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
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


        public void InsertClearedCheque(ChequeVoucherDTO cheque, DateTime clearedDate)
        {
            var whyNot = GetWhyInvalid(cheque);
            if (!whyNot.IsBlank())
                throw Bad.Insert(cheque, whyNot);

            var repo = FindRepo(clearedDate);
            var row  = cheque.ToPassbookRow(clearedDate);
            if (!repo.IsValidForInsert(row, out whyNot))
                throw Bad.Insert(row, whyNot);

            repo.Insert(row);
        }


        private string GetWhyInvalid(ChequeVoucherDTO cheque)
        {
            var req = cheque.Request;

            if (req == null)
                return "Request object should not be NULL.";

            if (req.BankAccountId != BankAccountID)
                return $"Expected Bank Acct ID to be [{BankAccountID}] but was [{req.BankAccountId}]";

            if (!req.Amount.HasValue)
                return "Requested amount should not be blank.";

            return string.Empty;
        }


        protected abstract ISimpleRepo<PassbookRowDTO> FindRepo(DateTime date);


        public void RecomputeBalancesFrom(DateTime date)
        {
            //todo: implement this
            //throw new NotImplementedException();
        }
    }
}
