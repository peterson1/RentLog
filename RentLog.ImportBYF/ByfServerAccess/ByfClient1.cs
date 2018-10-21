using CommonTools.Lib45.ThreadTools;
using CommonTools.Lib11.JsonTools;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfServerAccess
{
    public class ByfClient1
    {
        private ByfServerVM _server;
        private HttpClient  _httpC;


        public ByfClient1(ByfServerVM byfServerVM)
        {
            _server = byfServerVM;
            _httpC  = new HttpClient();
            _httpC.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "RmF0cyBDb21tb246M0U3MTgwMUY5NzI2NzUyOTM2NDIxMEE1RDAzODlFRTc2OEQ1MzM2MA==");
        }


        public async Task<bool> TestConnection()
        {
            var url = ToURL("api/entity_node/1");
            HttpResponseMessage rep;
            try
            {
                rep = await _httpC.GetAsync(url);
            }
            catch (Exception ex)
            {
                Alert.Show(ex, $"Connecting to {_server.URL}");
                return false;
            }
            if (rep.IsSuccessStatusCode) return true;
            Alert.Show($"Status [{(int)rep.StatusCode}]", rep.ReasonPhrase);
            return false;
        }


        private string ToURL(string relativeURL)
            => new Uri(new Uri(_server.URL), relativeURL).ToString();


        public async Task<List<dynamic>> GetViewsList(string viewsDisplayId)
        {
            var url  = ToURL("api/views/" + viewsDisplayId);
            var rep  = await _httpC.GetAsync(url);
            var json = await rep.Content.ReadAsStringAsync();
            return json.ReadJson<List<dynamic>>();
        }
    }
}
