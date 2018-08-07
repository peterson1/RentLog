using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CommonTools.Lib45.PrintTools.PrintPreviewer2
{
    public class PreviewWindowVM<TModel, TView> : MainWindowVMBase<TModel>, IPrintSettings
        where TView : FrameworkElement, new()
    {
        protected override string CaptionPrefix => "Print Preview";


        public PreviewWindowVM(TModel appArguments) : base(appArguments)
        {
            CurrentItemLabel = "-";
            PreviousItemCmd  = R2Command.Relay(LoadPreviousItem, _ => CanLoadPreviousItem(_), "Previous Item");
            NextItemCmd      = R2Command.Relay(LoadNextItem    , _ => CanLoadNextItem    (_), "Next Item");
        }


        public double  InchesWidth   { get; set; } = 8.5;
        public double  InchesHeight  { get; set; } = 11.0;

        public string      CurrentItemLabel  { get; set; }
        public IR2Command  PreviousItemCmd   { get; }
        public IR2Command  NextItemCmd       { get; }


        protected virtual void  LoadNextItem      () { }
        protected virtual void  LoadPreviousItem  () { }

        protected virtual bool CanLoadPreviousItem(object arg) => false;
        protected virtual bool CanLoadNextItem    (object arg) => false;


        protected override async void OnPrintClicked()
        {
            ShowWindow();
            await Task.Delay(500);
            var elm = GetPreviewWindow().mainPanel.Children[0];
            await Task.Delay(500);
            PrintPreviewer.FitTo(InchesWidth, InchesHeight, elm as FrameworkElement);
            CloseWindow();
        }


        public void ShowWindow()
        {
            this.Show<PrintPreviewer2Window1>();
            var win  = GetPreviewWindow();
            win.Top  = 30;
            win.Left = 30;
            FillMainPanel();
        }


        private void FillMainPanel()
        {
            var panl = GetPreviewWindow().mainPanel;
            panl.Children.Clear();
            var ctrl = new TView();
            panl.Children.Add(ctrl);
        }


        private PrintPreviewer2Window1 GetPreviewWindow()
            => GetWindowInstance() as PrintPreviewer2Window1;


        public IPrintSettings OrientationLandscape()
        {
            InchesWidth = 11;
            InchesHeight = 8.5;
            return this;
        }
    }
}
