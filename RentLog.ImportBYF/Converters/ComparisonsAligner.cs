using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters
{
    public static class ComparisonsAligner
    {
        public static int UnexpectedsCount;

        public static List<JsonComparer> AlignByIDs(this (List<IDocumentDTO>, List<IDocumentDTO>) tupl)
        {
            var bag   = new ConcurrentBag<JsonComparer>();
            var jobs  = new List<Action>();
            var list1 = tupl.Item1;
            var list2 = tupl.Item2;
            var maxId = GetMaxId(list1, list2);
            var minId = GetMinId(list1, list2, maxId);

            foreach (var item1 in list1)
            {
                jobs.Add(() =>
                {
                    var id1 = item1.Id;
                    var item2 = list2.SingleOrDefault(_ => _.Id == id1);
                    bag.Add(new JsonComparer(id1, item1, item2));
                });
            }
            jobs.Add(() => AlertUnexpectedItems(list1, list2));

            Parallel.Invoke(jobs.ToArray());
            var diffs = bag.ToList();
            diffs.RemoveAll(_ => _.BothNull);
            return diffs.OrderBy(_ => _.CommonKey).ToList();
        }


        private static void AlertUnexpectedItems(List<IDocumentDTO> list1, List<IDocumentDTO> list2)
        {
            UnexpectedsCount = 0;
            foreach (var item2 in list2)
            {
                if (!list1.Any(_ => _.Id == item2.Id))
                {
                    UnexpectedsCount++;
                    var cap = $"Unexpected item in list 2: [id: {item2.Id}]";
                    Alert.Show(cap, item2.ToString());
                }
            }
        }


        private static int GetMaxId(List<IDocumentDTO> list1, List<IDocumentDTO> list2)
        {
            var val1 = list1 == null ? 0 : !list1.Any() ? 0
                     : list1.Max(_ => _.Id);

            var val2 = list2 == null ? 0 : !list2.Any() ? 0
                     : list2.Max(_ => _.Id);

            return Math.Max(val1, val2);
        }


        private static int GetMinId(List<IDocumentDTO> list1, List<IDocumentDTO> list2, int minVal)
        {
            var val1 = list1 == null ? minVal : !list1.Any() ? minVal
                     : list1.Min(_ => _.Id);

            var val2 = list2 == null ? minVal : !list2.Any() ? minVal
                     : list2.Min(_ => _.Id);

            return Math.Min(val1, val2);
        }
    }
}
