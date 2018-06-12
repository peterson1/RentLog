using CommonTools.Lib11.DTOs;

namespace RentLog.DomainLib11.DTOs
{
    public enum GLAcctType
    {
        Asset,
        Liability,
        Equity,
        Income,
        Expense
    }

    public class GLAccountDTO : DocumentDTOBase
    {
        public GLAcctType  AccountType  { get; set; }
        public string      Name         { get; set; }


        public override string ToString() 
            => $"{Name}  [{AccountType}]";


        public static GLAccountDTO CashInBank(BankAccountDTO bankAccount) => new GLAccountDTO
        {
            AccountType = GLAcctType.Asset,
            Name        = $"Cash in Bank: {bankAccount.Name}"
        };
    }
}
