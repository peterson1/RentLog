using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using CommonTools.Lib45.UIExtensions;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib45.BaseViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class MainWindowVMBase<TArg>
    {
        public event EventHandler PrintClicked     = delegate { };
        public event EventHandler RefreshCompleted = delegate { };

        protected string RefreshingText = "Refreshing list of records...";
        protected abstract string CaptionPrefix { get; }

        private Window       _win;


        public MainWindowVMBase(TArg appArguments)
        {
            AppArgs        = appArguments;
            PrintCmd       = R2Command.Relay(OnPrintClicked, _ => !IsBusy, "Print");
            RefreshCmd     = R2Command.Async(DoRefresh, _ => !IsBusy, "Refresh");
            CloseWindowCmd = R2Command.Relay(CloseWindow, null, "Close Window");
            //SetCaption($"as {AppArgs?.Credentials?.NameAndRole ?? "Anonymous"}");
            SetCaption(".");
        }


        public TArg        AppArgs         { get; }
        public string      Caption         { get; private set; }
        public bool        IsBusy          { get; private set; }
        public string      BusyText        { get; private set; }
        public bool        ShouldClose     { get; protected set; }
        public string      WhyShouldClose  { get; protected set; }
        public IR2Command  PrintCmd        { get; }
        public IR2Command  RefreshCmd      { get; }
        public IR2Command  CloseWindowCmd  { get; }


        public void CloseWindow() => AsUI(() => _win?.Close());




        protected void SetCaption(string message)
        {
            try   { Caption = $"   {CaptionPrefix}   (v{CurrentExe.GetShortVersion()})    {message}"; }
            catch { Caption = $"   {CaptionPrefix}   {message}"; }
        }


        public void StartBeingBusy(string message)
        {
            IsBusy   = true;
            BusyText = message;
        }
        public void StopBeingBusy() => IsBusy = false;


        protected void AsUI(Action action)
        {
            if (_win == null)
            {
                try   { action.Invoke(); }
                catch { }
            }
            else
                UIThread.Run(action);
        }


        private async Task DoRefresh()
        {
            //if (IsBusy) return;//crud test fails with this uncommented
            StartBeingBusy(RefreshingText);

            //OnRefreshClicked();
            await Task.Run(() => OnRefreshClicked());
            await Task.Run(() => OnRefreshClickedAsync());

            StopBeingBusy();
            RefreshCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void ApplyWindowTheme     (Window win) { }
        protected virtual void OnRefreshClicked     () { }
        protected virtual void OnPrintClicked       () => PrintClicked?.Invoke(this, EventArgs.Empty);
        protected virtual Task OnRefreshClickedAsync() => Task.Delay(0);
        protected virtual void OnWindowLoaded       () { }
        public    virtual Task OnWindowClosing(CancelEventArgs cancelEvtArgs) => Task.Delay(0);
        public void ClickRefresh() => RefreshCmd.ExecuteIfItCan();


        public bool? Show<T>(bool hideWindow = false, bool showModal = false) 
            where T: Window, new()
        {
            if (ShouldClose)
            {
                Alert.ShowModal("Exiting ...", WhyShouldClose);
                CurrentExe.Shutdown();
                return false;
            }

            _win = new T();
            ApplyWindowTheme(_win);
            _win.DataContext = this;
            _win.MakeDraggable();
            _win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _win.SnapsToDevicePixels = true;
            _win.Loaded  += (s, e) => OnWindowLoaded();
            _win.Closing += async (s, e) => await OnWindowClosing(e);

            #if DEBUG
            _win.SetToCloseOnEscape();
            _win.WindowState = WindowState.Normal;
            #endif

            if (showModal)
                return _win.ShowDialog();
            else
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _win.ShowTemporarilyOnTop();
#pragma warning restore CS4014
                if (hideWindow) _win.Hide();
                return null;
            }
        }


        protected void ReturnDialogResult(bool? result)
        {
            if (_win == null) return;
            if (_win.IsActive)
                _win.DialogResult = result;
            CloseWindow();
        }


        public bool AllFieldsValid()
            => _win?.AllFieldsValid() ?? true;
    }
}
