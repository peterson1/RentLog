using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane
{
    public class MasterDataPaneVM
    {
        public MasterDataPaneVM(MainWindowVM2 main)
        {
            Converters = new ConvertersListVM(main);
        }

        public ConvertersListVM Converters { get; }
    }
}
