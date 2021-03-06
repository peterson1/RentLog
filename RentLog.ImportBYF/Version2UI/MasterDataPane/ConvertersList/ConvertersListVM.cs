﻿using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.ImportBYF.Converters.BankAccountConverters;
using RentLog.ImportBYF.Converters.CollectorConverters;
using RentLog.ImportBYF.Converters.GLAccountConverters;
using RentLog.ImportBYF.Converters.LeaseConverters;
using RentLog.ImportBYF.Converters.SectionConverters;
using RentLog.ImportBYF.Converters.StallConverters;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList
{
    [AddINotifyPropertyChangedInterface]

    public class ConvertersListVM : UIList<IConverterRow>
    {
        public ConvertersListVM(MainWindowVM2 main)
        {
            Add(new GLAccountConverter2   (main));
            Add(new BankAccountConverter2 (main));
            Add(new CollectorConverter2   (main));
            Add(new SectionConverter2     (main));
            Add(new StallConverter2       (main));
            Add(new LeaseConverter2       (main));

            main.ByfServer.ConnectedToServer 
                += async (s, e) => await RefreshAll();
        }


        private async Task RefreshAll()
        {
            foreach (var row in this)
            {
                row.RefreshCmd.ExecuteIfItCan();
                await Task.Delay(500);
            }
        }
    }
}
