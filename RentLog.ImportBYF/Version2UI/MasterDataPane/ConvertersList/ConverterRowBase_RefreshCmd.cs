using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList
{
    public static class ConverterRowBase_RefreshCmd
    {
        public static async Task GetDifferences<T>(this ConverterRowBase<T> row) where T : class, IDocumentDTO
        {
            row.StartBeingBusy("Getting row differences ...");
            row.ErrorText = null;
            await Task.Delay(1);
            var diffs = await row.QueryThenCompare();
            UIThread.Run(() => row.DiffRows.SetItems(diffs));
            row.UpdateCounts();
            row.ShowFirstDiff();
            row.StopBeingBusy();
        }


        private static void ShowFirstDiff<T>(this ConverterRowBase<T> row) where T : class, IDocumentDTO
        {
            var diff1 = row.DiffRows.FirstOrDefault(_ => _.IsTheSame == false);
            if (diff1 == null) return;
            row.LogError(diff1.Difference);
        }


        private static async Task<IEnumerable<JsonComparer>> QueryThenCompare<T>(this ConverterRowBase<T> row) where T : class, IDocumentDTO
        {
            var byfRecs = await row.GetByfRecords();
            var rntRecs = row.SafeGetRntRecords();
            return row.AlignRecords(byfRecs, rntRecs);
        }


        private static async Task<List<T>> GetByfRecords<T>(this ConverterRowBase<T> row) where T : class, IDocumentDTO
        {
            var casteds = new List<T>();
            var dynamics = await row.QueryByfServer();
            if (dynamics == null) return null;

            foreach (var dyn in dynamics)
            {
                if (TryCastToRNT(row, dyn, out T casted))
                    casteds.Add(casted);
                else
                    return null;
            }
            return casteds;
        }


        private static bool TryCastToRNT<T>(ConverterRowBase<T> row, dynamic dynamic, out T casted) where T : class, IDocumentDTO
        {
            try
            {
                casted = row.CastToRNT(dynamic);
                return true;
            }
            catch (Exception ex)
            {
                var obj = dynamic as object;
                row.LogError(obj.ToJson(true));
                row.LogError(ex.Info(true, true));
                casted = default(T);
                return false;
            }
        }


        private static async Task<List<dynamic>> QueryByfServer<T>(this ConverterRowBase<T> row) where T : class, IDocumentDTO
        {
            try
            {
                await row.BeforeByfQuery();
                return await row.GetViewsList(row.ViewsDisplayID);
            }
            catch (Exception ex)
            {
                row.LogError(ex.Info(true, true));
                return null;
            }
        }


        private static List<T> SafeGetRntRecords<T>(this ConverterRowBase<T> row) where T : class, IDocumentDTO
        {
            try
            {
                return row.GetRntRecords(row.Main.AppArgs);
            }
            catch (Exception ex)
            {
                row.LogError(ex.Info(true, true));
                return null;
            }
        }


        private static List<JsonComparer> AlignRecords<T>(this ConverterRowBase<T> row, List<T> byfRecs, List<T> rntRecs) where T : class, IDocumentDTO
        {
            try
            {
                return row.AlignByIDs(byfRecs, rntRecs);
            }
            catch (Exception ex)
            {
                row.LogError(ex.Info(true, true));
                return null;
            }
        }
    }
}
