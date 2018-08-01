using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.LanguageTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using static RentLog.ChequeVouchers.Properties.Settings;

namespace RentLog.ChequeVouchers.VoucherReqsTab.ChequeVoucherPrints
{
    public class PrintPreviewWindowVM : PrintPreviewVMBase<ChequeVoucherDTO, ITenantDBsDir, PrintPreviewWindow>
    {
        public PrintPreviewWindowVM(ChequeVoucherDTO dto, ITenantDBsDir dir) : base(dto, dir)
        {
            
            AllocationTotals.SetItems(GetAllocationsSummary(dto));
            ReviewerName = Default.ReviewerName;
            ApproverName = Default.ApproverName;
        }


        public int?       CheckNumberOrNull => CurrentItem.ChequeNumber == 0 ? (int?)null : CurrentItem.ChequeNumber;
        public DateTime?  CheckDateOrNull   => CurrentItem.ChequeDate == DateTime.MinValue ? (DateTime?)null : CurrentItem.ChequeDate;
        public string     ReviewerName    { get; set; }
        public string     ApproverName    { get; set; }
        public string     AmountInWords  => CurrentItem.Request.Amount?.ToPesoWords();
        public UIList<AccountAllocation> AllocationTotals { get; } = new UIList<AccountAllocation>();


        protected override FrameworkElement GetElementToPrint()
        {
            var win = GetWindowInstance() as PrintPreviewWindow;
            return win?.printable;
        }


        private IEnumerable<AccountAllocation> GetAllocationsSummary(ChequeVoucherDTO dto) 
            => new List<AccountAllocation> { new AllocationsTotal(dto.Request.Allocations) };
    }


    public static class ChequeVoucherPrint
    {
        public static void Preview(ChequeVoucherDTO dto, ITenantDBsDir dir) 
            => new PrintPreviewWindowVM(dto, dir).Show();


        public static void Preview(FundRequestDTO req, ITenantDBsDir dir)
            => Preview(new ChequeVoucherDTO { Request = req }, dir);
    }
}
