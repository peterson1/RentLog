using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.ImportBYF.Converters.BankAccountConverters;
using RentLog.ImportBYF.Converters.CollectorConverters;
using RentLog.ImportBYF.Converters.GLAccountConverters;
using RentLog.ImportBYF.Converters.SectionConverters;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList
{
    [AddINotifyPropertyChangedInterface]

    public class ConvertersListVM : UIList<IConverterRow>
    {
        public ConvertersListVM(MainWindowVM2 main)
        {
            Add(new GLAccountConverter2(main));
            Add(new BankAccountConverter2(main));
            Add(new CollectorConverter2(main));
            Add(new SectionConverter2(main));
        }


        public async Task RefreshAll()
        {
            foreach (var row in this)
            {
                row.RefreshCmd.ExecuteIfItCan();
                await Task.Delay(500);
            }
        }
    }
}
