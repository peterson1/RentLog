using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.BankDeposits
{
    [AddINotifyPropertyChangedInterface]
    public class BankDepositsVM : EncoderListVMBase<BankDepositDTO>
    {
        private BankDepCrudVM _crud;


        public BankDepositsVM(MainWindowVM main) : base(main.ColxnsDB.BankDeposits, main)
        {
            _crud = new BankDepCrudVM(main.ColxnsDB?.BankDeposits, main.AppArgs);
        }


        protected override string ListTitle    => "Bank Deposits";
        protected override void   AddNewItem() => _crud.EncodeNewDraftCmd.ExecuteIfItCan();
        protected override Func<BankDepositDTO, decimal> SummedAmount => _ => _.Amount;
        protected override void LoadRecordForEditing(BankDepositDTO rec) => _crud.EditCurrentRecord(rec);
    }
}
