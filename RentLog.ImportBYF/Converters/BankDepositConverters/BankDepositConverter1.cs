using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.BankDepositConverters
{
    public class BankDepositConverter1 : DailyTxnConverterBase<BankDepositDTO>
    {
        protected override string DisplayId => "bank_deposits_list?display_id=page_1";


        public BankDepositConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        protected override BankDepositDTO CastToDTO(dynamic byf) => new BankDepositDTO
        {
            Id          = byf.nid,
            Amount      = byf.Amount,
            DepositDate = byf.DateDeposited,
            Description = _byfCache.Term((int)byf.DepositCodeTid),
            DocumentRef = byf.ReferenceNum,
            Remarks     = byf.Remarks,
            BankAccount = _rntCache.BankAcctById((int)byf.BankAcctNid)
        };


        protected override void ReplaceInColxnsDB(IEnumerable<BankDepositDTO> rntDTOs, ICollectionsDB colxnsDB) 
            => colxnsDB.BankDeposits.DropAndInsert(rntDTOs, true, false);
    }
}
