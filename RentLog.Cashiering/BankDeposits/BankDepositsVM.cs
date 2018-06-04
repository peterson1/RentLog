using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.BankDeposits
{
    [AddINotifyPropertyChangedInterface]
    public class BankDepositsVM : EncoderListVMBase<BankDepositDTO>
    {
        protected override string ListTitle => "Bank Deposits";


        public BankDepositsVM(MainWindowVM main) : base(main.ColxnsDB.BankDeposits, main)
        {
        }


        protected override Func<BankDepositDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
