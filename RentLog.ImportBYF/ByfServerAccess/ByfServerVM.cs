using PropertyChanged;
using CommonTools.Lib11.CollectionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Version2UI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CommonTools.Lib11.ExceptionTools;

namespace RentLog.ImportBYF.ByfServerAccess
{
    [AddINotifyPropertyChangedInterface]
    public class ByfServerVM
    {
        private MainWindowVM2            _main;
        private ByfClient1               _client;
        //private Dictionary<Type, string> _urlByType;

        public ByfServerVM(MainWindowVM2 mainWindowVM2)
        {
            _main      = mainWindowVM2;
            _client    = new ByfClient1(this);
            //_urlByType = FillUrlsList();
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
        }


        //private Dictionary<Type, string> FillUrlsList() => new Dictionary<Type, string>
        //{
        //    { typeof(GLAccountDTO), "gl_accounts_list?display_id=page_2" }
        //};


        public string  URL       { get; private set; }
        public bool    IsOnline  { get; private set; }


        public async Task SetURL(string newUrl)
        {
            IsOnline = false;
            URL      = newUrl;
            IsOnline = await IsServerOnline();
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


        //public async Task<List<dynamic>> GetListOf<T>()
        //{
        //    if (!_urlByType.TryGetValue(typeof(T), out string url))
        //        throw No.Match<string>("key", typeof(T).Name);

        //    return await _client.GetViewsList(url);
        //}
    }
}
