using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.BankDeposits
{
    [AddINotifyPropertyChangedInterface]
    public class BankDepositsVM : EncoderListVMBase<BankDepositDTO, AppArguments>
    {
        protected override string ListTitle => "Bank Deposits";


        public BankDepositsVM(ISimpleRepo<BankDepositDTO> repository, AppArguments appArguments) : base(repository, appArguments)
        {
        }


        protected override Func<BankDepositDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
