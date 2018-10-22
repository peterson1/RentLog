using CommonTools.Lib11.DynamicTools;
using PropertyChanged;
using RentLog.ImportBYF.Converters.MiscellaneousConverters;
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
        private ByfClient1    _client;


        public ByfServerVM(MainWindowVM2 mainWindowVM2)
        {
            _main   = mainWindowVM2;
            _client = new ByfClient1(this);
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
        }


        public string    URL             { get; private set; }
        public bool      IsOnline        { get; private set; }
        public DateTime  FirstMarketDate { get; private set; }
        public DateTime  LastPostedDate  { get; private set; }


        public async Task SetURL(string newUrl)
        {
            IsOnline        = false;
            URL             = newUrl;
            IsOnline        = await IsServerOnline();
            var period      = await GetMinMaxDates();
            FirstMarketDate = period.Min;
            LastPostedDate  = period.Max;
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
            var ok = await _client.TestConnection();
            var suffx = ok ? "(online)" : "!! Failed to connect !!";
            _main.SetCaption($"{_main.AppArgs.Credentials.NameAndRole} {URL} {suffx}");
            return ok;
        }


        internal Task<List<dynamic>> GetViewsList(string viewsDisplayID)
            => _client.GetViewsList(viewsDisplayID);
    }
}
