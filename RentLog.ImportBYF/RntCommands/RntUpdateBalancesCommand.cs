using RentLog.ImportBYF.Converters.BalanceAdjConverters;
using RentLog.ImportBYF.Version2UI.LeaseBalancesPane.LeasesList;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.RntCommands
{
    public static class RntUpdateBalancesCommand
    {
        public static async Task UpdateBalances(this LeaseRowVM row)
        {
            var mkt   = row.MainWindow.AppArgs.MarketState;
            var conv  = new BalanceAdjConverter1(row.Lease, row.MainWindow);
            var byfs  = await conv.GetByfRecords();
            conv.ReplaceAll(byfs, mkt);
        }
    }
}
