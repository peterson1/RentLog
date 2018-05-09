using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.SoaViewer.MainWindow
{
    public class SoaViewerVM : MainWindowVMBase<AppArguments>
    {
        protected override string CaptionPrefix => "Statement of Account";

        private IDailyBillsRepo _repo;


        public SoaViewerVM(LeaseDTO leaseDTO, AppArguments appArguments) : base(appArguments)
        {
            Lease = leaseDTO;
            _repo = AppArgs.Balances.GetRepo(Lease);
            SetCaption($"[{Lease.Id}]  {Lease.TenantAndStall}");
            ClickRefresh();
        }


        public LeaseDTO               Lease { get; }
        public UIList<DailyBillsRow>  Rows  { get; } = new UIList<DailyBillsRow>();


        protected override void OnRefreshClicked()
        {
            Rows.SetItems(GetBillRows());
        }


        private IEnumerable<DailyBillsRow> GetBillRows()
        {
            var colctrs = AppArgs.MarketState.Collectors.ToDictionary();
            var rows    = _repo.GetAll ()
                               .Select (_ => new DailyBillsRow(Lease, _, colctrs))
                               .ToList ();
            return rows;
        }
    }


    public static class SoaViewer
    {
        public static void Show(LeaseDTO lse, AppArguments args)
        {
            if (lse == null || args == null) return;
            new SoaViewerVM(lse, args).Show<SoaViewerWindow>();
        }
    }
}
