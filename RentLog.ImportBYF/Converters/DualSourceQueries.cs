using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Environment;

namespace RentLog.ImportBYF.Converters
{
    public static class DualSourceQueries
    {
        //private const string CACHE_DIR = "BasicAuthBulkCacheReader";


        public static (List<IDocumentDTO>, List<IDocumentDTO>) QueryBothSources(this ComparisonsListBase compList)
        {
            var byfDir               = compList.MainWindow.CacheDir;
            var rntDir               = compList.AppArgs;
            List<IDocumentDTO> byfs  = null;
            List<IDocumentDTO> rnts  = null;
            Parallel.Invoke(() => byfs = QueryBYF(compList, byfDir),
                            () => rnts = compList.GetListFromRNT(rntDir));
            return (byfs, rnts);
        }


        private static List<IDocumentDTO> QueryBYF(ComparisonsListBase compList, string cacheDir)
        {
            List<object> byfList = null;
            try
            {
                byfList = compList.GetListFromBYF(cacheDir);
            }
            catch (Exception ex)
            {
                Alert.Show(ex, "Querying list from BYF");
                return null;
            }
            return ConcurrentCast(compList, byfList);
        }


        private static List<IDocumentDTO> ConcurrentCast(ComparisonsListBase compList, List<object> byfList)
        {
            var jobs = new List<Action>();
            var bag = new ConcurrentBag<IDocumentDTO>();

            foreach (var rec in byfList)
                jobs.Add(() => bag.Add(SafeCastByfToDTO(compList, rec)));

            Parallel.Invoke(jobs.ToArray());

            return bag.ToList();
        }


        private static IDocumentDTO SafeCastByfToDTO(ComparisonsListBase compList, object byf)
        {
            try
            {
                return compList.CastByfToDTO(byf);
            }
            catch (Exception ex)
            {
                return new DocumentDTOBase
                {
                    Id = compList.GetByfId(byf),
                    Remarks = ex.Info(true)
                };
            }
        }


        //private static string FindCacheDir()
        //    => SpecialFolder.LocalApplicationData.Path(CACHE_DIR);
    }
}
