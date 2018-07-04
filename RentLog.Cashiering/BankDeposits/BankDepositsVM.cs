using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.BankDeposits
{
    [AddINotifyPropertyChangedInterface]
    public class BankDepositsVM : EncoderListVMBase<BankDepositDTO>
    {
        public BankDepositsVM(MainWindowVM main) : base(main.ColxnsDB.BankDeposits, main)
        {
            Crud = new BankDepCrudVM(main.ColxnsDB?.BankDeposits, main.AppArgs);
        }


        public BankDepCrudVM Crud { get; }


        protected override string ListTitle    => "Bank Deposits";
        protected override void   AddNewItem() => Crud.EncodeNewDraftCmd.ExecuteIfItCan();
        protected override Func<BankDepositDTO, decimal> SummedAmount => _ => _.Amount;
        protected override void LoadRecordForEditing(BankDepositDTO rec) => Crud.EditCurrentRecord(rec);
    }
}
