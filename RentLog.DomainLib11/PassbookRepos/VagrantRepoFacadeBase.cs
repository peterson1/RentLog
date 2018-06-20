using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.DomainLib11.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.PassbookRepos
{
    public abstract class VagrantRepoFacadeBase : IPassbookRowsRepo
    {
        public VagrantRepoFacadeBase(int bankAccountId)
        {
            BankAccountID = bankAccountId;
        }


        public int BankAccountID { get; }

        protected abstract ISimpleRepo<PassbookRowDTO> ConnectToDB(string databasePath);
        protected abstract string GetDatabasePath(DateTime date);


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


        public void RecomputeBalancesFrom(DateTime startDate)
        {
            throw new NotImplementedException($"RecomputeBalancesFrom({startDate:d MMM yyyy})");
            //foreach (var date in startDate.EachDayUpToNow())
            //{
            //
            //}
        }


        public List<PassbookRowDTO> RowsFor(DateTime date)
            => FindRepo(date).Find(_ 
                => _.DateOffset == date.DaysSinceMin());


        private ISimpleRepo<PassbookRowDTO> FindRepo(DateTime date)
            => ConnectToDB(GetDatabasePath(date));


        public List<PassbookRowDTO> RowsFor(DateTime startDate, DateTime endDate)
        {
            var min   = startDate.DaysSinceMin();
            var max   = endDate.DaysSinceMin();
            var rows  = new List<PassbookRowDTO>();
            var paths = startDate.EachDayUpTo(endDate)
                                 .Select  (_ => GetDatabasePath(_))
                                 .GroupBy (_ => _)
                                 .Select  (_ => _.First()).ToList();

            foreach (var path in paths)
                rows.AddRange(ConnectToDB(path)
                    .Find(_ => _.DateOffset >= min
                            && _.DateOffset <= max));
            return rows;
        }
    }
}
