﻿using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45.SoaViewers.CellViewer;
using RentLog.DomainLib45.SoaViewers.PrintLayouts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.DomainLib45.SoaViewers.MainWindow
{
    public class SoaViewerVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "Statement of Account";


        public SoaViewerVM(LeaseDTO leaseDTO, ITenantDBsDir appArguments) : base(appArguments)
        {
            Lease = leaseDTO;
            SetCaption($"[{Lease.Id}]  {Lease.TenantAndStall}");
            Rows.ItemOpened += (s, e) => OnItemOpened(e);
            ClickRefresh();
        }


        public LeaseDTO               Lease     { get; }
        public BillCode               BillCode  { get; set; } = BillCode.Other;
        public UIList<DailyBillsRow>  Rows      { get; } = new UIList<DailyBillsRow>();
        public DailyBillsRow          FirstRow  { get; private set; }


        private IEnumerable<DailyBillsRow> GetBillRows()
        {
            var colctrs = AppArgs.MarketState.Collectors.ToDictionary();
            var repo    = AppArgs.Balances.GetRepo(Lease);
            return repo.GetAll ()
                       .Select (_ => new DailyBillsRow(Lease, _, colctrs))
                       .ToList ();
        }


        private void OnItemOpened(DailyBillsRow e)
        {
            if (BillCode == BillCode.Other) return;
            var changd = SoaCellViewer.Show(Lease, e.Date, e.DTO.For(BillCode), AppArgs);
            if (changd == true) ClickRefresh();
        }



        //protected override void OnRefreshClicked() => Rows.SetItems(GetBillRows());
        protected override async Task OnRefreshClickedAsync()
        {
            IEnumerable<DailyBillsRow> rows = null;
            await Task.Run(() => rows = GetBillRows());
            UIThread.Run(() => Rows.SetItems(rows));
            FirstRow = Rows.FirstOrDefault();
        }

        protected override void OnPrintClicked  () => SoaPrintVM.Print(this);
    }
}
