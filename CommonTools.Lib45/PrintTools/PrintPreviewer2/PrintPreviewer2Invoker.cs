using System.Windows;

namespace CommonTools.Lib45.PrintTools.PrintPreviewer2
{
    public struct PrintPreviewer2
    {
        public static PrintPreviewer2<T> Preview<T>(T viewModel) 
            => new PrintPreviewer2<T>(viewModel);
    }


    public class PrintPreviewer2<TModel>
    {
        private TModel _model;


        public PrintPreviewer2(TModel viewModel)
        {
            _model = viewModel;
        }


        public IPrintSettings On<TView>()
            where TView : FrameworkElement, new()
        {
            var vm = new PreviewWindowVM<TModel, TView>(_model);
            vm.ShowWindow();
            return vm;
        }
    }
}
