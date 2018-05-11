using CommonTools.Lib45.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.DomainLib45.SoaViewer.CellViewer
{
    public class SoaCellViewerVM : MainWindowVMBase<AppArguments>
    {
        protected override string CaptionPrefix => "SoA Cell";


        public SoaCellViewerVM(AppArguments appArguments) : base(appArguments)
        {
        }

    }
}
