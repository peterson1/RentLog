using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.SoaViewers.AdjustmentsCRUD;
using System;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib45.SoaViewers.CellViewer
{
    public class SoaCellViewerVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "SoA Cell";


        public SoaCellViewerVM(LeaseDTO leaseDTO, DateTime businessDate, BillState billState, ITenantDBsDir args) : base(args)
        {
            Lease       = leaseDTO;
            Date        = businessDate;
            Bill        = billState;
            Adjustments = new AdjustmentsListVM(Lease.Id, Bill.BillCode, Date, AppArgs);
            Adjustments.TotalSumChanged += (s, e) => ReturnDialogResult(true);
        }


        public LeaseDTO           Lease        { get; }
        public DateTime           Date         { get; }
        public BillState          Bill         { get; }
        public AdjustmentsListVM  Adjustments  { get; }
    }


    public static class SoaCellViewer
    {
        public static bool? Show(LeaseDTO leaseDTO, DateTime businessDate, BillState billState, ITenantDBsDir args)
            => new SoaCellViewerVM(leaseDTO, businessDate, billState, args).Show<SoaCellViewerWindow>(showModal: true);
    }
}
