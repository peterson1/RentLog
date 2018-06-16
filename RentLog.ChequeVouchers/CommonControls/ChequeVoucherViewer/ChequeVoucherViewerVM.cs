using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.VoucherRules;

namespace RentLog.ChequeVouchers.CommonControls.ChequeVoucherViewer
{
    public class ChequeVoucherViewerVM : MainWindowVMBase<ITenantDBsDir>
    {
        public ChequeVoucherViewerVM(ChequeVoucherDTO chequeVoucherDTO, ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            DTO = chequeVoucherDTO;
            if (!DTO.Request.HasCashInBankEntry())
                DTO.Request.Allocations.AddCashInBankEntry(AppArgs.CurrentBankAcct, DTO.Request.Amount.Value);
        }


        public ChequeVoucherDTO DTO { get; }


        protected override string CaptionPrefix => "Cheque Voucher";


        public static void Show(ChequeVoucherDTO dto, ITenantDBsDir dir)
            => new ChequeVoucherViewerVM(dto, dir)
                .Show<ChequeVoucherViewerWindow>();
    }
}
