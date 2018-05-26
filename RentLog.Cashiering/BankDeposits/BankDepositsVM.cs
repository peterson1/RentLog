using System;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;

namespace RentLog.Cashiering.BankDeposits
{
    [AddINotifyPropertyChangedInterface]
    public class BankDepositsVM : EncoderListVMBase<BankDepositDTO, AppArguments>
    {
        public BankDepositsVM(ISimpleRepo<BankDepositDTO> repository, AppArguments appArguments) : base(repository, appArguments, false)
        {
            Caption = "Bank Deposits";
        }

        protected override Func<BankDepositDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
