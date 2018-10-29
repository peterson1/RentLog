using CommonTools.Lib11.EventHandlerTools;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane
{
    public class MasterDataPaneVM
    {
        public event EventHandler OnAllLeasesMatch = delegate { };


        public MasterDataPaneVM(MainWindowVM2 main)
        {
            Converters = new ConvertersListVM(main);
        }


        public ConvertersListVM Converters { get; }


        public void RaiseAllLeasesMatch() => OnAllLeasesMatch.Raise();
    }
}
