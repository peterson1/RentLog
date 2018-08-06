using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CommonTools.Lib45.PrintTools.PrintPreviewer2
{
    public class PreviewWindowVM<TModel, TView> : MainWindowVMBase<TModel>
        where TView : FrameworkElement, new()
    {
        protected override string CaptionPrefix => "Print Preview";


        public PreviewWindowVM(TModel appArguments) : base(appArguments)
        {
            CurrentItemLabel = "-";
            PreviousItemCmd  = R2Command.Relay(LoadPreviousItem, _ => CanLoadPreviousItem(_), "Previous Item");
            NextItemCmd      = R2Command.Relay(LoadNextItem    , _ => CanLoadNextItem    (_), "Next Item");
        }


        public string      CurrentItemLabel  { get; set; }
        public IR2Command  PreviousItemCmd   { get; }
        public IR2Command  NextItemCmd       { get; }


        protected virtual void  LoadNextItem      () { }
        protected virtual void  LoadPreviousItem  () { }

        protected virtual bool CanLoadPreviousItem(object arg) => false;
        protected virtual bool CanLoadNextItem    (object arg) => false;


        protected override async void OnPrintClicked()
        {
            this.Show<PrintPreviewer2Window1>();
            var elm = FillMainPanel();
            await Task.Delay(1000);
            PrintPreviewer.FitTo(8.5, 11, elm);
            CloseWindow();
        }


        public TView FillMainPanel()
        {
            var panl = GetPreviewWindow().mainPanel;
            panl.Children.Clear();
            var ctrl = new TView();
            panl.Children.Add(ctrl);
            return ctrl;
        }


        private PrintPreviewer2Window1 GetPreviewWindow()
            => GetWindowInstance() as PrintPreviewer2Window1;
    }
}
