using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.Lib45.FileSystemTools
{
    public static class TempFile
    {
        public static string NewNonExistentPath()
        {
            var fPath = Path.GetTempFileName();
            File.Delete(fPath);
            return fPath;
        }
    }
}
