using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.BankDeposits
{
    [AddINotifyPropertyChangedInterface]
    public class BankDepositsVM : EncoderListVMBase<BankDepositDTO>
    {
        protected override string ListTitle => "Bank Deposits";


        public BankDepositsVM(ISimpleRepo<BankDepositDTO> repository, ITenantDBsDir tenantDBsDir) : base(repository, tenantDBsDir)
        {
        }


        protected override Func<BankDepositDTO, decimal> SummedAmount => _ => _.Amount;
    }
}
