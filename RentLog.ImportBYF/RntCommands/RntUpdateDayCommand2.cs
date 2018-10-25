﻿using CommonTools.Lib11.ExceptionTools;
using RentLog.ImportBYF.Converters.AmbulantColxnConverters;
using RentLog.ImportBYF.Converters.IntendedColxnConverters;
using RentLog.ImportBYF.Converters.OtherColxnConverters;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;
using System;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.RntCommands
{
    public static class RntUpdateDayCommand2
    {
        public static async Task UpdateRNT (this PeriodRowVM row, DateTime date)
        {
            try
            {
                row.Remarks = "Updating Intended Collections ...";
                await new IntendedColxnConverter2(row).Rewrite(date);
                row.Remarks = "Updating Ambulant Collections ...";
                await new AmbulantColxnConverter2(row).Rewrite(date);
                row.Remarks = "Updating Other Collections ...";
                await new OtherColxnConverter2(row).Rewrite(date);
                //todo: continue from here
                //UpdateCashierColxns(row);
                //UpdateBankDeposits(row);
                await row.RefreshCmd.RunAsync();
            }
            catch (Exception ex)
            {
                row.Remarks = ex.Info(true, true);
            }
        }
    }
}
