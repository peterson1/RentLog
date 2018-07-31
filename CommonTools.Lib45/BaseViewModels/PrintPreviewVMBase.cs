using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.PrintTools;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib45.BaseViewModels
{
    public abstract class PrintPreviewVMBase<TItem, TArg, TWin> : MainWindowVMBase<TArg>
        where TWin : Window, new()
    {
        protected override string CaptionPrefix => "Print Preview";


        public PrintPreviewVMBase(TItem currentItem, TArg appArguments) : base(appArguments)
        {
            CurrentItem     = currentItem;
            PreviousItemCmd = R2Command.Relay(LoadPreviousItem, _ => CanLoadPreviousItem(_), "Previous Item");
            NextItemCmd     = R2Command.Relay(LoadNextItem    , _ => CanLoadNextItem    (_), "Next Item");
            //PrintCmd.ExecuteIfItCan();
        }


        public TItem       CurrentItem      { get; private set; }
        public IR2Command  PreviousItemCmd  { get; }
        public IR2Command  NextItemCmd      { get; }


        protected abstract FrameworkElement GetElementToPrint();


        protected virtual void  LoadNextItem      () { }
        protected virtual void  LoadPreviousItem  () { }

        protected virtual bool CanLoadPreviousItem(object arg) => false;
        protected virtual bool CanLoadNextItem    (object arg) => false;


        protected override async void OnPrintClicked()
        {
            this.Show<TWin>();
            await Task.Delay(1000);
            PrintPreviewer.FitTo(8.5, 11, GetElementToPrint());
            CloseWindow();
        }


        public void Show() => this.Show<TWin>();
    }
}
