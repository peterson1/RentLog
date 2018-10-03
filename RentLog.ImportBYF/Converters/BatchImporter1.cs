using CommonTools.Lib11.DTOs;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters
{
    public static class BatchImporter1
    {
        public static async Task ImportAll(this ComparisonsListBase compList)
        {
            var win = compList.MainWindow;
            win.StartBeingBusy($"Importing all {win.PickedListName} ...");
            await Task.Delay(1);
            await Task.Run(() =>
            {
                try
                {
                    var recs = compList.Select(_ => _.Document1 as IDocumentDTO);
                    compList.ReplaceAll(recs, win.AppArgs.MarketState);
                }
                catch (Exception ex)
                {
                    Alert.Show(ex, $"Importing all {win.PickedListName}");
                }
            });
            win.StopBeingBusy();
            win.ClickRefresh();
        }
    }
}
