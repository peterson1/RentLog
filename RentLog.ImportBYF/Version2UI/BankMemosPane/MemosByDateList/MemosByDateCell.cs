using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Version2UI.BankMemosPane.MemosByDateList
{
    public class MemosByDateCell
    {
        public Dictionary<int, List<PassbookRowDTO>> DTOs { get; set; }

        public bool IsQueried => DTOs != null;

        public int?     KVPsCount  => DTOs?.Count;
        public int?     AcctId1    => DTOs?.Keys?.FirstOrDefault();
        public int?     List1Count => DTOs?.Values?.FirstOrDefault()?.Count;
        public decimal? List1Sum   => DTOs?.Values?.FirstOrDefault()?.Sum(_ => _.Amount);
        public decimal? TotalSum   => GetTotalSum();


        private decimal? GetTotalSum()
        {
            if (DTOs == null) return null;
            if (!DTOs.Any()) return null;
            var sum = 0M;

            foreach (var list in DTOs.Values)
                sum += list.Sum(_ => _.Amount);

            return sum;
        }


        public bool Matches(MemosByDateCell othr, out string whyNot)
        {
            if (this.KVPsCount != othr.KVPsCount) return False(whyNot = "KVP Counts mismatch");
            foreach (var kvp in this.DTOs)
            {
                if (!othr.DTOs.ContainsKey(kvp.Key))
                    return False(whyNot = $"Other does not contain BankAcctId [{kvp.Key}]");

                var thisList = kvp.Value;
                var othrList = othr.DTOs[kvp.Key];

                if (thisList.Count != othrList.Count)
                    return False(whyNot = "Inner list counts mismatched.");
            }
            whyNot = "";
            return true;
        }


        private bool False(string reason) => false;
    }
}
