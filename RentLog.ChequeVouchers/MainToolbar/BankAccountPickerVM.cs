using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System.Linq;

namespace RentLog.ChequeVouchers.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class BankAccountPickerVM
    {
        private ITenantDBsDir _dir;

        public BankAccountPickerVM(ITenantDBsDir tenantDBsDir)
        {
            _dir = tenantDBsDir;

            BankAccounts.SetItems
                (_dir.MarketState.BankAccounts.GetAll());

            SelectedBankAccount = BankAccounts.FirstOrDefault();
        }


        public UIList<BankAccountDTO>  BankAccounts         { get; } = new UIList<BankAccountDTO>();
        public BankAccountDTO          SelectedBankAccount  { get; set; }


        public void OnSelectedBankAccountChanged() 
            => _dir.CurrentBankAcct = SelectedBankAccount;
    }
}
