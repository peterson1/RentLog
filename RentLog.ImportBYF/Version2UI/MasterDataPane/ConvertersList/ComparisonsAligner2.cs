using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList
{
    public static class ComparisonsAligner2
    {
        internal static List<JsonComparer> AlignByIDs <T>(this ConverterRowBase<T> row, 
            List<T> byfRecs, List<T> rntRecs)
            where T : class, IDocumentDTO
        {
            if (byfRecs == null) return null;
            var bag   = new ConcurrentBag<JsonComparer>();
            var jobs  = new List<Action>();
            var maxId = GetMaxId(byfRecs, rntRecs);
            var minId = GetMinId(byfRecs, rntRecs, maxId);

            foreach (var byf in byfRecs)
            {
                jobs.Add(() =>
                {
                    var rnt = rntRecs.SingleOrDefault(_ => _.Id == byf.Id);
                    bag.Add(new JsonComparer(byf.Id, byf, rnt));
                });
            }
            jobs.Add(() 
                => row.Unexpecteds = CountUnexpecteds(byfRecs, rntRecs));

            Parallel.Invoke(jobs.ToArray());
            var diffs = bag.ToList();
            diffs.RemoveAll(_ => _.BothNull);
            return diffs.OrderBy(_ => _.CommonKey).ToList();
        }


        private static int CountUnexpecteds<T>(List<T> byfRecs, List<T> rntRecs) where T : IDocumentDTO
        {
            var unexpecteds = 0;
            foreach (var rnt in rntRecs)
            {
                if (!byfRecs.Any(_ => _.Id == rnt.Id))
                {
                    unexpecteds++;
                    var cap = $"Unexpected item in RNT list: [id: {rnt.Id}]";
                    Alert.Show(cap, rnt.ToString());
                }
            }
            return unexpecteds;
        }


        private static int GetMaxId<T>(List<T> list1, List<T> list2)
            where T : IDocumentDTO
        {
            var val1 = list1 == null ? 0 : !list1.Any() ? 0
                     : list1.Max(_ => _.Id);

            var val2 = list2 == null ? 0 : !list2.Any() ? 0
                     : list2.Max(_ => _.Id);

            return Math.Max(val1, val2);
        }


        private static int GetMinId<T>(List<T> list1, List<T> list2, int minVal)
            where T : IDocumentDTO
        {
            var val1 = list1 == null ? minVal : !list1.Any() ? minVal
                     : list1.Min(_ => _.Id);

            var val2 = list2 == null ? minVal : !list2.Any() ? minVal
                     : list2.Min(_ => _.Id);

            return Math.Min(val1, val2);
        }
    }
}
