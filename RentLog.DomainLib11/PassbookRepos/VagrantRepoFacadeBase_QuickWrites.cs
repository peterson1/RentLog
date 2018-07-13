using CommonTools.Lib11.DatabaseTools;
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
        private IKeyValueStore _meta;


        public VagrantRepoFacadeBase(int bankAccountId, IKeyValueStore dbMetadata)
        {
            _meta         = dbMetadata;
            BankAccountID = bankAccountId;
        }


        public int   BankAccountID   { get; }


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

        public bool IsValidForUpdate(PassbookRowDTO rec, out string whyInvalid)
            => FindRepo(rec.TransactionDate).IsValidForUpdate(rec, out whyInvalid);

        public bool IsValidForDelete(PassbookRowDTO rec, out string whyInvalid)
            => FindRepo(rec.TransactionDate).IsValidForDelete(rec, out whyInvalid);


        public int Insert(PassbookRowDTO rec)
            => FindRepo(rec.TransactionDate).Insert(rec);

        public bool Update(PassbookRowDTO rec)
            => throw new InvalidOperationException("Don't call IPassbookRowsRepo.Update(). Instead, call Delete() then Insert()");

        public bool Delete(PassbookRowDTO rec)
        {
            var ok = FindRepo(rec.TransactionDate).Delete(rec);
            if (!ok) throw new Exception("IPassbookRowsRepo.Delete returned False");
            return ok;
        }
    }
}
