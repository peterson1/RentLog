using CommonTools.Lib11.StringTools;
using System.Diagnostics;
using System.IO;
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

            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);

            var clipbrdVal = (string)Clipboard.GetData(DataFormats.Text);

            dg.UnselectAllCells();
            dg.SelectionMode = origCanSelect;

            if (filePath.IsBlank())
                filePath = Path.GetTempFileName() + ".csv";

            using (var writr = new StreamWriter(filePath))
                writr.WriteLine(clipbrdVal.Replace(',', ' '));

            Process.Start(filePath);
        }
    }
}
