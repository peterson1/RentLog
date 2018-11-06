using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.EventHandlerTools;
using PropertyChanged;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.Converters.MiscellaneousConverters;
using RentLog.ImportBYF.DailyTransactions;
using RentLog.ImportBYF.Version2UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfServerAccess
{
    [AddINotifyPropertyChangedInterface]
    public class ByfServerVM
    {
        private MainWindowVM2 _main;

        public event EventHandler ConnectedToServer = delegate { };
        public event EventHandler<(DateTime Min, DateTime Max)> GotMinMaxDates = delegate { };


        public ByfServerVM(MainWindowVM2 mainWindowVM2)
        {
            _main  = mainWindowVM2;
            Client = new ByfClient1(this);
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
        }


        public ByfClient1  Client          { get; }
        public string      URL             { get; private set; }
        public bool        IsOnline        { get; private set; }
        public DateTime    FirstMarketDate { get; private set; }
        public DateTime    LastPostedDate  { get; private set; }


        public async Task SetURL(string newUrl)
        {
            IsOnline        = false;
            URL             = newUrl;
            IsOnline        = await IsServerOnline();
            if (!IsOnline) return;
            ConnectedToServer.Raise();
            var period      = await GetMinMaxDates();
            FirstMarketDate = period.Min;
            LastPostedDate  = period.Max;
            GotMinMaxDates.Raise(period);
        }


        private async Task<(DateTime Min, DateTime Max)> GetMinMaxDates()
        {
            var viewsId = FiscalDaysConverter1.ViewsDisplayID;
            var vList   = await GetViewsList(viewsId);
            var item1   = vList.First();
            return (As.Date(item1.min),
                    As.Date(item1.max));
        }


        private async Task<bool> IsServerOnline()
        {
            _main.SetCaption($"testing server “{URL}” ...");
            var ok = await Client.TestConnection();
            var suffx = ok ? "(online)" : "!! Failed to connect !!";
            _main.SetCaption($"{_main.AppArgs.Credentials.NameAndRole} {URL} {suffx}");
            return ok;
        }


        //public Task<BillAmounts> QueryLeaseBalances(LeaseDTO lease)
        //{
        //    throw new NotImplementedException();
        //}


        public async Task<DailyTransactionCell> QueryPeriodCell(DateTime date)
        {
            var lseColxns      = await Client.GetLeaseColxnsTotal   (date);
            var ambulantColxns = await Client.GetAmbulantColxnsTotal(date);
            var otherColxns    = await Client.GetOtherColxnsTotal   (date);
            var cashierColxns  = await Client.GetCashierColxnsTotal (date);
            var deposits       = await Client.GetBankDepositsTotal  (date);
            return new DailyTransactionCell
            {
                TotalCollections = lseColxns + ambulantColxns 
                                 + cashierColxns + otherColxns,
                TotalDeposits    = deposits
            };
        }


        internal Task<List<dynamic>> GetViewsList(string viewsDisplayID)
            => Client.GetViewsList(viewsDisplayID);

        internal Task<List<dynamic>> GetViewsList(string viewsDisplayID, DateTime date)
            => Client.GetViewsList(viewsDisplayID, date);
    }
}
