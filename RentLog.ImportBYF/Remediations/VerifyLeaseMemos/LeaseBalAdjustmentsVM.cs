using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;

namespace RentLog.ImportBYF.Remediations.VerifyLeaseMemos
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseBalAdjustmentsVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "Lease Bal Adj Verifier";


        public LeaseBalAdjustmentsVM(LeaseDTO lease, ITenantDBsDir appArguments) : base(appArguments)
        {
            Lease = lease;
            Rows  = new LeaseBalAdjsList(this);
        }


        public LeaseDTO          Lease  { get; }
        public LeaseBalAdjsList  Rows   { get; }
    }
}
