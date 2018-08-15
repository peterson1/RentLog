using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.JsonTools;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Threading.Tasks;

namespace CommonTools.Lib45.GoogleTools
{
    public static class FirebaseCustomExtensions
    {
        public static FirebaseClient CreateClient(this FirebaseCredentials creds)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(creds.ApiKey));
            var opts = new FirebaseOptions
            {
                AuthTokenAsyncFactory = async () =>
                {
                    var link = await auth.SignInWithEmailAndPasswordAsync(creds.Email, creds.Password);
                    return link.FirebaseToken;
                }
            };
            return new FirebaseClient(creds.BaseURL, opts);
        }


        public static async Task<(bool IsSuccessful, string Response)> TryPost<T>(this FirebaseClient client, T payload, string path)
        {
            try
            {
                var json = payload.ToJson();
                var fObj = await client.Child(path).PostAsync(json);
                return (true, fObj.Key);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
