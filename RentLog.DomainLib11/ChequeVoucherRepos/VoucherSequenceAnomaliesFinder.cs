using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class VoucherSequenceAnomalies
    {
        public static string Find(IEnumerable<FundRequestDTO> items)
        {
            var gaps = FindGaps(items);
            var dups = FindDuplicates(items);
            return string.Join(L.f, gaps.Concat(dups));
        }


        private static List<string> FindGaps(IEnumerable<FundRequestDTO> items)
        {
            var gaps = new List<string>();
            var nums = items.Select(_ => _.SerialNum).ToList();
            var max  = nums.Max();

            for (int i = 1; i <= max; i++)
            {
                if (!nums.Contains(i))
                    gaps.Add($"Missing CV# {i}");
            }
            return gaps;
        }


        private static List<string> FindDuplicates(IEnumerable<FundRequestDTO> items)
            => items.GroupBy(_ => _.SerialNum)
                    .Where  (_ => _.Count() > 1)
                    .Select (_ => _.First())
                    .Select (_ => $"Duplicate CV# {_.SerialNum}" )
                    .ToList ();
    }
}
