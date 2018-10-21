using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList
{
    public static class ConverterRowBase_UpdateRntCmd
    {
        public static async Task UpdateRnt<T>(this ConverterRowBase<T> row) where T : class, IDocumentDTO
        {
            row.StartBeingBusy($"Importing all {row.Label} ...");
            await Task.Delay(1);
            await Task.Run(() =>
            {
                try
                {
                    var recs = row.DiffRows.Select(_ => _.Document1 as T);
                    row.ReplaceAll(recs, row.Main.AppArgs.MarketState);
                }
                catch (Exception ex)
                {
                    row.LogError(ex.Info(true, true));
                }
            });
            await Task.Delay(500);
            row.StopBeingBusy();
            row.RefreshCmd.ExecuteIfItCan();
        }


        public static bool CanUpdateRnt<T>(this ConverterRowBase<T> row) where T : class, IDocumentDTO
        {
            if (row.Unexpecteds != 0) return false;
            if (row.DiffsCount  == 0) return false;
            return true;
        }
    }
}
