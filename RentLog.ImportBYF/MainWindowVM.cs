using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.FileSystemTools;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.ImportBYF.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Environment;


namespace RentLog.ImportBYF
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Import BYF";


        public MainWindowVM(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
        }


        public UIList<string>      ListNames        { get; } = new UIList<string>();
        public int                 PickedListIndex  { get; set; } = -1;
        public ConvertedsListBase  PickedList       { get; private set; }
        public string              PickedListName   => ListNames[PickedListIndex];


        protected override void OnWindowLoaded()
        {
            if (PickedListIndex == -1)
                PickedListIndex = 0;
        }


        protected override async Task OnRefreshClickedAsync()
        {
            var dir = SpecialFolder.LocalApplicationData.Path();

            IDictionary<long, ReportModels.Lease> byfDict = null;
            await Task.Run(() 
                => byfDict = CacheReader2.getLeases(dir));

            //Casted.SetItems(byfDict.Values
            //    .Select(_ => CastBYF(_)));
        }


        private LeaseDTO CastBYF(ReportModels.Lease byf) => new LeaseDTO
        {
            ContractStart = byf.ContractStart,
            ContractEnd = byf.ContractEnd,
        };
    }
}
