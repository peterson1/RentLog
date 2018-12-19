using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Remediations.VerifyLeaseMemos
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseBalAdjsList : UIList<LeaseBalAdjRow>
    {
        public LeaseBalAdjsList(LeaseBalAdjustmentsVM windowVM)
        {
        }
    }
}
