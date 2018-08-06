using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.LanguageTools;
using CommonTools.Lib45.PrintTools.PrintPreviewer2;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using static RentLog.ChequeVouchers.Properties.Settings;

namespace RentLog.ChequeVouchers.VoucherReqsTab.ChequeVoucherPrints
{
    public class PrintPreviewWindowVM //: PrintPreviewVMBase<ChequeVoucherDTO, ITenantDBsDir, PrintPreviewWindow>
    {
        public PrintPreviewWindowVM(ChequeVoucherDTO dto, ITenantDBsDir dir) //: base(dto, dir)
        {
            AppArgs = dir;
            CurrentItem = dto;
            AllocationTotals.SetItems(GetAllocationsSummary(dto));
            ReviewerName = Default.ReviewerName;
            ApproverName = Default.ApproverName;
        }


        public ITenantDBsDir AppArgs { get; }
        public ChequeVoucherDTO   CurrentItem   { get; }
        public int?       CheckNumberOrNull => CurrentItem.ChequeNumber == 0 ? (int?)null : CurrentItem.ChequeNumber;
        public DateTime?  CheckDateOrNull   => CurrentItem.ChequeDate == DateTime.MinValue ? (DateTime?)null : CurrentItem.ChequeDate;
        public string     ReviewerName    { get; set; }
        public string     ApproverName    { get; set; }
        public string     AmountInWords  => CurrentItem.Request.Amount?.ToPesoWords();
        public UIList<AccountAllocation> AllocationTotals { get; } = new UIList<AccountAllocation>();


        private IEnumerable<AccountAllocation> GetAllocationsSummary(ChequeVoucherDTO dto) 
            => new List<AccountAllocation> { new AllocationsTotal(dto.Request.Allocations) };
    }


    public static class ChequeVoucherPrint
    {
        public static void Preview(ChequeVoucherDTO dto, ITenantDBsDir dir)
        {
            var vm = new PrintPreviewWindowVM(dto, dir);
            PrintPreviewer2.Preview(vm).On<PrintLayout1>();
        }


        public static void Preview(FundRequestDTO req, ITenantDBsDir dir)
            => Preview(new ChequeVoucherDTO { Request = req }, dir);
    }
}
