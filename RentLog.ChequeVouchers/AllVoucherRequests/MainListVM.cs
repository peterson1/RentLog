using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.ChequeVouchers.CommonControls.ChequeVoucherViewer;
using RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ChequeVouchers.AllVoucherRequests
{
    [AddINotifyPropertyChangedInterface]
    public class MainListVM : FilteredSavedListVMBase<FundRequestDTO, FilterVM, ITenantDBsDir>
    {
        public MainListVM(ISimpleRepo<FundRequestDTO> simpleRepo, ITenantDBsDir appArguments, bool doReload = true) : base(simpleRepo, appArguments, doReload)
        {
        }


        protected override List<FundRequestDTO> QueryItems(ISimpleRepo<FundRequestDTO> db) 
            => base.QueryItems(db).Where(_ => _.BankAccountId == AppArgs.CurrentBankAcct.Id)
                                  .OrderByDescending(_ => _.SerialNum)
                                  .ToList();


        protected override void OnItemOpened(FundRequestDTO e)
        {
            if (AppArgs.CanEditInactiveRequest(false))
                new FundRequestCrudVM(AppArgs)
                    .EditCurrentRecord(e);
        }


        public override async void ReloadFromDB()
        {
            base.ReloadFromDB();

            await Task.Delay(1000 * 2);

            var anomalies = VoucherSequenceAnomalies.Find(ItemsList);

            if (!anomalies.IsBlank())
                Alert.Show("Check Voucher Anomalies", anomalies);
        }
    }
}
