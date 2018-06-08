using System;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests
{
    public class FundReqsListVM : FilteredSavedListVMBase<FundRequestDTO, FundReqsFilterVM, ITenantDBsDir>
    {
        private const string CMD_LABEL = "Input Cheque Details";

        public FundReqsListVM(ITenantDBsDir dir) 
            : base(dir.Vouchers.ActiveRequests, dir, false)
        {
            Caption = "For Cheque Preparation";
            InputChequeCmd = R2Command.Relay(InputChequeDetails, _ => CanInputCheque(), CMD_LABEL);
        }


        public IR2Command InputChequeCmd { get; }


        private bool CanInputCheque()
        {
            if (!AppArgs.CanInputChequeDetails(false))
                return CantDo($"[{AppArgs.Credentials.Roles}] NOT authorized");

            if (!TryGetPickedItem(out FundRequestDTO dto))
                return CantDo("No selected item");

            if (!dto.Amount.HasValue)
                return CantDo("Requested Amount is blank");

            InputChequeCmd.CurrentLabel = CMD_LABEL;
            return true;
        }


        private bool CantDo(string whyNot)
        {
            InputChequeCmd.CurrentLabel = $"{CMD_LABEL} -- {whyNot}";
            return false;
        }


        private void InputChequeDetails()
        {
            //Alert.Show(ItemsList.CurrentItem.Purpose);
            var req = ItemsList.CurrentItem;
            var dte = DateTime.Now;
            var num = 123;
            AppArgs.Vouchers.SetAs_Prepared(req, dte, num);
        }


        protected override Func<FundRequestDTO, decimal> SummedAmount => _ => _.Amount ?? 0;
    }
}
