using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.DomainLib11.Validations;
using System;

namespace RentLog.DomainLib11.PassbookRepos
{
    public abstract partial class VagrantRepoFacadeBase : IPassbookRowsRepo
    {
        public VagrantRepoFacadeBase(int bankAccountId)
        {
            BankAccountID = bankAccountId;
        }


        public int BankAccountID { get; }


        public void InsertClearedCheque(ChequeVoucherDTO cheque, DateTime clearedDate)
        {
            var whyNot = cheque.WhyInvalidForSetAsCleared(BankAccountID);
            if (!whyNot.IsBlank())
                throw Bad.Insert(cheque, whyNot);

            var repo = FindRepo(clearedDate);
            var row  = cheque.ToPassbookRow(clearedDate);
            repo.Insert(row);
        }


        public void InsertDepositedColxn(BankDepositDTO deposit, DateTime colxnDate)
        {
            var whyNot = deposit.WhyInvalidForColxnDeposit(BankAccountID);
            if (!whyNot.IsBlank())
                throw Bad.Insert(deposit, whyNot);

            var repo = FindRepo(deposit.DepositDate);
            var row  = deposit.ToPassbookRow(colxnDate);
            repo.Insert(row);
        }


        public bool IsValidForInsert(PassbookRowDTO draft, out string whyInvalid)
            => FindRepo(draft.TransactionDate).IsValidForInsert(draft, out whyInvalid);

        public bool IsValidForUpdate(PassbookRowDTO record, out string whyInvalid)
            => FindRepo(record.TransactionDate).IsValidForUpdate(record, out whyInvalid);


        public int Insert(PassbookRowDTO rec)
            => FindRepo(rec.TransactionDate).Insert(rec);

        public bool Update(PassbookRowDTO rec)
            => FindRepo(rec.TransactionDate).Update(rec);
    }
}
