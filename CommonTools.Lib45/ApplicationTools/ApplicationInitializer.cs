using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib45.ApplicationTools
{
    public static class AppInitializer
    {
        public static Action<Exception, string> OnError = (ex, ctx) 
            => Alert.Show(ex, ctx);


        public static void Initialize<TArg>(this Application app, Action<TArg> actionOnArguments)
            where TArg : new()
        {
            HandleGlobalErrors();
            ThisThread.SetShortDateFormat("d MMM yyyy");
            TArg args = default(TArg);
            ExitOnError(() => args = new TArg(), "Parsing arguments");
            ExitOnError(() => actionOnArguments(args), "Consuming arguments");
        }


        private static void ExitOnError(Action action, string context)
        {
            try { action.Invoke(); }
            catch (Exception ex)
            {
                OnError(ex, context);
                CurrentExe.Shutdown();
            }
        }


        private static void HandleGlobalErrors()
        {
            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                OnError(e.Exception, "Application.Current");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                OnError(e.Exception, "TaskScheduler");
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                OnError(e.ExceptionObject as Exception, "AppDomain.CurrentDomain");
                // application terminates after above
            };
        }
    }
}
