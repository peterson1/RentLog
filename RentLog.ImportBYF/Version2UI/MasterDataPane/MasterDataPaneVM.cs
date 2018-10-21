using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
