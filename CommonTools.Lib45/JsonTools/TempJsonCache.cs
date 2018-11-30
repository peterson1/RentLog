using CommonTools.Lib11.JsonTools;
using System.IO;
using System.Text;

namespace CommonTools.Lib45.JsonTools
{
    public class TempJsonCache : JsonCacheBase
    {
        private string _tmp = Path.GetTempPath();


        protected override string GetFilePath(string key)
            => Path.Combine(_tmp, $"{key}.json");


        protected override void WriteTextFile(string filePath, string content)
            => File.WriteAllText(filePath, content, Encoding.UTF8);

        protected override bool   FileExists   (string filePath) => File.Exists(filePath);
        protected override string ReadTextFile (string filePath) => File.ReadAllText(filePath);
    }
}
