using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib11.JsonTools
{
    public class JsonComparer
    {
        public JsonComparer(object commonKey, object document1, object document2)
        {
            CommonKey = commonKey;
            IsTheSame = SeeIfTheSame(Document1 = document1,
                                     Document2 = document2);
        }


        public object  CommonKey    { get; }
        public object  Document1    { get; }
        public object  Document2    { get; }
        public bool?   IsTheSame    { get; }
        public string  Json1        { get; private set; }
        public string  Json2        { get; private set; }
        public string  CompareError { get; private set; }
        public string  Difference   { get; private set; }

        public bool BothNull => Document1 == null
                             && Document2 == null;


        private bool? SeeIfTheSame(object doc1, object doc2)
        {
            try
            {
                Difference = GetDifference(doc1, doc2);
                return Difference.IsBlank();
            }
            catch (Exception ex)
            {
                CompareError = ex.Info();
                return null;
            }
        }


        private string GetDifference(object doc1, object doc2)
        {
            Json1        = doc1.ToJson(true);
            Json2        = doc2.ToJson(true);
            var jsLines1 = Json1.SplitByLine();
            var jsLines2 = Json2.SplitByLine();
            var maxLines = Math.Max(jsLines1.Length, jsLines2.Length);
            var diffs    = new List<string>();

            for (int i = 0; i < maxLines; i++)
            {
                var line1 = jsLines1.ElementAtOrDefault(i);
                var line2 = jsLines2.ElementAtOrDefault(i);

                if (line1 != line2)
                    diffs.Add($"[{line1}]" + " - vs - " 
                            + $"[{line2}]");
            }

            return diffs.Any() ? string.Join(L.f, diffs)
                               : null;
        }
    }
}
