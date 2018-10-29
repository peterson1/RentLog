using CommonTools.Lib11.ExceptionTools;
using RentLog.ImportBYF.Version2UI.LeaseBalancesPane.LeasesList;
using System;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.RntCommands
{
    public static class RntUpdateBalancesCommand
    {
        public static async Task UpdateRNT(this LeaseRowVM row)
        {
            try
            {
                row.Remarks = "Updating Rent balances ...";
                await Task.Delay(1);
                row.Remarks = "Updating Rights balances ...";
                await row.RefreshCmd.RunAsync();
            }
            catch (Exception ex)
            {
                row.Remarks = ex.Info(true, true);
            }
        }
    }
}
