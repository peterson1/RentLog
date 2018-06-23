using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.ThreadTools;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib45.ExcelTools
{
    public static class DataGridToExcel
    {
        public static void ExportToExcel(this DataGrid dg, string filePath = null)
        {
            var origCanSelect = dg.SelectionMode;
            dg.SelectionMode = DataGridSelectionMode.Extended;
            dg.SelectAllCells();

            string clipbrdVal = "";
            bool isOK = true;
            try
            {
                dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, dg);
                clipbrdVal = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            }
            catch (COMException)
            {
                Alert.ShowModal("Exporting to Excel failed", "Another app might be using the system's clipboard");
                isOK = false;
            }

            dg.UnselectAllCells();
            dg.SelectionMode = origCanSelect;
            if (!isOK) return;

            if (filePath.IsBlank())
                filePath = Path.GetTempFileName() + ".csv";

            using (var writr = new StreamWriter(filePath))
                writr.WriteLine(clipbrdVal);

            Process.Start(filePath);
        }
    }
}
