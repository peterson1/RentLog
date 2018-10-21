using CommonTools.Lib45.ThreadTools;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommonTools.Lib45.HttpTools
{
    public static class HttpClientAccessors
    {
        //public static async Task<bool> IsSuccessful(this string url, bool alertIfNot = false)
        //{
        //    HttpResponseMessage rep;
        //    try
        //    {
        //        using (var client = new HttpClient())
        //            rep = await client.GetAsync(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (alertIfNot)
        //            Alert.Show(ex, $"Testing {url}");
        //        return false;
        //    }

        //    if (rep.IsSuccessStatusCode) return true;

        //    if (alertIfNot)
        //        Alert.Show($"Status: {rep.StatusCode}", rep.ReasonPhrase);

        //    return false;
        //}

    }
}
