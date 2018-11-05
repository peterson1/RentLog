using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.RntCommands;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.BankDepositConverters
{
    public class BankDepositConverter2 : DailyTxnConverterBase2<BankDepositDTO>
    {
        public BankDepositConverter2(PeriodRowVM periodRowVM) : base(periodRowVM)
        {
        }


        protected override BankDepositDTO CastToDTO(dynamic byf) => new BankDepositDTO
        {
            Id          = byf.nid,
            Amount      = byf.amount,
            DepositDate = byf.datedeposited,
            Description = _byfCache.Term(As.ID(byf.depositcodetid)),
            DocumentRef = byf.referencenum,
            Remarks     = byf.remarks,
            BankAccount = _rntCache.BankAcctById(As.ID(byf.bankacctnid))
        };


        protected override Task<List<dynamic>> GetRawBYFsList(ByfClient1 client, DateTime date)
            => client.GetRawByfBankDeposits(date);


        protected override void ReplaceInColxnsDB(IEnumerable<BankDepositDTO> rntDTOs, ICollectionsDB colxnsDB)
        {
            colxnsDB.BankDeposits.DropAndInsert(rntDTOs, true, false);
            //_rntDir.UpdateAccountPassbooks(rntDTOs, colxnsDB.Date);
            colxnsDB.AddDepsToPassbookIfNeeded(rntDTOs, _rntDir);
        }
    }
}
