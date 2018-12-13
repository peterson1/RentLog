using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Version2UI.JournalVouchersPane.JVsByDateList
{
    [AddINotifyPropertyChangedInterface]
    public class JVsByDateCell
    {
        public List<JournalVoucherDTO> DTOs { get; set; }

        public int?     SerialSum   => DTOs?.Sum(_ => _.SerialNum);
        public int?     DateSum     => DTOs?.Sum(_ => _.DateOffset);
        public int?     AllocsCount => DTOs?.Sum(_ => _.Allocations.Count);
        public int?     AllocAccts  => DTOs?.Sum(_ => _.Allocations.Sum(a => a.Account.Id));
        public decimal? AllocsSum   => DTOs?.Sum(_ => _.Allocations.Sum(a => a.SubAmount));
        public decimal? AmountSum   => DTOs?.Sum(_ => _.Amount);


        private bool False (string reason) => false;
        public  bool IsQueried             => DTOs != null;


        public bool Matches(JVsByDateCell othr, out string whyNot)
        {
            if (this.DTOs.Count  != othr.DTOs.Count ) return False(whyNot = "DTOs Count mismatch");
            if (this.SerialSum   != othr.SerialSum  ) return False(whyNot = "SerialSum mismatch");
            if (this.DateSum     != othr.DateSum    ) return False(whyNot = "DateSum mismatch");
            if (this.AllocsCount != othr.AllocsCount) return False(whyNot = "AllocsCount mismatch");
            if (this.AllocAccts  != othr.AllocAccts ) return False(whyNot = "AllocAccts mismatch");
            if (this.AllocsSum   != othr.AllocsSum  ) return False(whyNot = "AllocsSum mismatch");
            if (this.AmountSum   != othr.AmountSum  ) return False(whyNot = "AmountSum mismatch");
            whyNot = "";
            return true;
        }
    }
}
