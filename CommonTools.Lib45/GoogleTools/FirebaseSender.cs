using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib45.FileSystemTools;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonTools.Lib45.GoogleTools
{
    public class FirebaseSender
    {
        private FirebaseCredentials _key;

        public FirebaseSender(FirebaseCredentials firebaseCredentials)
        {
            _key = firebaseCredentials;
        }


        public async Task Post(Exception exception, string context)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_key.ApiKey));
            var opts = new FirebaseOptions
            {
                AuthTokenAsyncFactory = async () =>
                {
                    var link = await auth.SignInWithEmailAndPasswordAsync(_key.Email, _key.Password);
                    return link.FirebaseToken;
                }
            };
            var client = new FirebaseClient(_key.BaseURL, opts);
            var report = new ErrorReport(_key, exception, context).ToJson();
            var path   = ToPath(_key.Email);
            var resp   = await client.Child(path).PostAsync(report);
        }


        private string ToPath(string email)
        {
            email = email.Replace('@', '_');
            email = email.Replace('.', '_');
            return $"errors/{email}";
        }


        public static async Task Post(Exception exception, string context, FirebaseCredentials credentials)
        {
            var sendr = new FirebaseSender(credentials);
            try   { await sendr.Post(exception, context); }
            catch { }
        }


        public class ErrorReport
        {
            public ErrorReport() { }

            public ErrorReport(FirebaseCredentials usr, Exception exception, string context)
            {
                HumanName  = usr.HumanName;
                Roles      = usr.Roles;
                Email      = usr.Email;
                Context    = context;
                ExeVersion = CurrentExe.ShortNameAndVersion();
                HumanTime  = DateTime.Now.ToString("d-MMM-yyyy h:mm tt");
                Details    = exception.Info(true, true)
                                .Split(new[] { "\r\n", "\r", "\n" }, 
                                StringSplitOptions.None).ToList();
            }

            public string        HumanName  { get; set; }
            public string        Roles      { get; set; }
            public string        Email      { get; set; }
            public string        Context    { get; set; }
            public string        ExeVersion { get; set; }
            public string        HumanTime  { get; set; }
            public List<string>  Details    { get; set; }
        }
    }
}
