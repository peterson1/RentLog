using CommonTools.Lib45.ExcelTools;
using OfficeOpenXml.Style;
using RentLog.DomainLib11.Reporters;
using System;

namespace RentLog.DomainLib45.Reporters
{
    public partial class GLRecapExcelWriter
    {
        private void WriteReportSummarySheet(GLRecapReport glRecapReport)
        {
            var subTitle = "Report Summary";
            _src = glRecapReport;
            _xl  = new CellWriter1ByRow("GL_Recap", subTitle);

            WriteSheetHeader(subTitle);

            var labelCol = 2;
            var colSpan  = 8;
            WriteRepSmryRowLabels(labelCol, colSpan);
            WriteTotalsRowLabel(labelCol, colSpan);

            var nextCol = labelCol + colSpan;
            var rowCount = _src.Count;

            WriteRepSmryAmounts(nextCol, "Debit", _ => _.TotalDebits);
            SummarizeCurrentColumn(rowCount);
            
            WriteRepSmryAmounts(nextCol + 1, "Credit", _ => _.TotalCredits);
            SummarizeCurrentColumn(rowCount);

            SetRowHeights(rowCount);
        }


        private void WriteRepSmryRowLabels(int labelCol, int colSpan)
        {
            _xl.MoveTo(H2_ROW, labelCol);
            _xl.WriteMergedH2("account type", 1, colSpan);
            _xl.MoveTo(N1_ROW, null);

            foreach (var cat in _src)
            {
                var rng = _xl.WriteMergedText(cat.AccountType.ToString(), 1,
                            colSpan, ExcelHorizontalAlignment.Left);
                rng.Style.Indent = 1;
            }
        }


        private void WriteRepSmryAmounts(int colNum, string colLabel, 
            Func<GLRecapCategory, decimal?> getter)
        {
            _xl.MoveTo(H2_ROW, colNum);
            _xl.CurrentCol.Width = 15;
            _xl.WriteH2(colLabel);

            _xl.MoveTo(N1_ROW, null);

            foreach (var cat in _src)
                _xl.WriteNumber(getter(cat));
        }
    }
}
