using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentLog.DomainLib11.DTOs;

namespace RentLog.ImportBYF.Version2UI.JournalVouchersPane.JVsByDateList
{
    public class JVsByDateCell
    {
        public List<JournalVoucherDTO> DTOs { get; set; }


        private bool False (string reason) => false;
        public  bool IsQueried             => DTOs != null;


        public bool Matches(JVsByDateCell othr, out string whyNot)
        {
            if (this.DTOs.Count != othr.DTOs.Count) return False(whyNot = "DTOs Count mismatch");
            whyNot = "";
            return true;
        }
    }
}
