using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public LeaseDTO Lease { get; }
    }
}
